using Market.Domain.Entities;
using Market.Infrastructure.Data;
using Market.Domain.DTOs;
using Market.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Market.Domain.Implementations;

public class BatchServices : IBatchServices
{
    private readonly AppDbContext _context;

    public BatchServices(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BatchResponseDto> CreateBatchAsync(CreateBatchDto dto)
    {
        var product = await _context.Products.FindAsync(dto.ProductId);
        if (product == null)
            throw new KeyNotFoundException($"Produto com ID {dto.ProductId} não encontrado.");

        var batch = new Batch
        {
            Registration = dto.Registration,
            FabricationDate = dto.FabricationDate,
            ValidityDate = dto.ValidityDate,
            Amount = dto.Amount,
            ProductId = dto.ProductId
        };

        // Atualiza o estoque total do produto com a quantidade deste novo lote
        product.Stock += dto.Amount;

        _context.Batches.Add(batch);
        await _context.SaveChangesAsync();

        return MapToDto(batch);
    }

    public async Task<BatchResponseDto> GetBatchByIdAsync(int id)
    {
        var batch = await _context.Batches
            .Include(b => b.Product)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (batch == null)
            throw new KeyNotFoundException($"Lote com ID {id} não encontrado.");

        return MapToDto(batch);
    }

    public async Task<IEnumerable<BatchResponseDto>> GetAllBatchesAsync()
    {
        var batches = await _context.Batches
            .AsNoTracking()
            .Include(b => b.Product)
            .ToListAsync();

        return batches.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<BatchResponseDto>> GetBatchesByProductIdAsync(int productId)
    {
        var batches = await _context.Batches
            .AsNoTracking()
            .Include(b => b.Product)
            .Where(b => b.ProductId == productId)
            .ToListAsync();

        return batches.Select(MapToDto).ToList();
    }

    public async Task<BatchResponseDto> UpdateBatchAsync(int id, UpdateBatchDto dto)
    {
        var batch = await _context.Batches
            .Include(b => b.Product)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (batch == null)
            throw new KeyNotFoundException($"Lote com ID {id} não encontrado.");

        var product = await _context.Products.FindAsync(dto.ProductId);
        if (product == null)
            throw new KeyNotFoundException($"Produto com ID {dto.ProductId} não encontrado.");

        // Calcula a diferença e reflete no estoque do produto e no lote
        var difference = dto.Amount - batch.Amount;
        if (difference > 0)
        {
            batch.IncreaseAmount(difference);
            batch.Product.Stock += difference;
        }
        else if (difference < 0)
        {
            var reduction = Math.Abs(difference);
            batch.DecreaseAmount(reduction);
            batch.Product.Stock -= reduction;
        }

        batch.Registration = dto.Registration;
        batch.FabricationDate = dto.FabricationDate;
        batch.ValidityDate = dto.ValidityDate;
        batch.ProductId = dto.ProductId;

        await _context.SaveChangesAsync();

        return MapToDto(batch);
    }

    public async Task DeleteBatchAsync(int id)
    {
        var batch = await _context.Batches
            .Include(b => b.Product)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (batch == null)
            throw new KeyNotFoundException($"Lote com ID {id} não encontrado.");

        // Desconta do estoque geral a quantidade do lote deletado
        if (batch.Product != null)
            batch.Product.Stock = Math.Max(0, batch.Product.Stock - batch.Amount);

        _context.Batches.Remove(batch);
        await _context.SaveChangesAsync();
    }

    private static BatchResponseDto MapToDto(Batch batch)
    {
        return new BatchResponseDto
        {
            Id = batch.Id,
            Registration = batch.Registration,
            FabricationDate = batch.FabricationDate,
            ValidityDate = batch.ValidityDate,
            Amount = batch.Amount,
            IsExpired = batch.IsExpired(),
            ProductId = batch.ProductId,
            ProductName = batch.Product != null ? batch.Product.Name : string.Empty
        };
    }
}