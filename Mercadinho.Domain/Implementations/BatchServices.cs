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

    public async Task CreateBatchAsync(CreateBatchDto dto)
    {

        var batch = new Batch
        {
            Registration = dto.Registration,
            FabricationDate = dto.FabricationDate,
            ValidityDate = dto.ValidityDate,
            ProductId = dto.ProductId
        };

        _context.Batches.Add(batch);
        await _context.SaveChangesAsync();
    }

    public async Task<BatchResponseDto> GetBatchByIdAsync(Guid id)
    {
        Batch? batch = await _context.Batches
            .Include(b => b.Product)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (batch == null)
            throw new KeyNotFoundException($"Lote com ID {id} não encontrado.");

        return MapToDto(batch);
    }

    public async Task<IEnumerable<BatchResponseDto>> GetAllBatchesAsync()
    {
        List<Batch> batches = await _context.Batches
            .AsNoTracking()// Não vai armazenar a busca em cache
            .Include(b => b.Product)
            .ToListAsync();

        return batches.Select(MapToDto).ToList();
    }

    public async Task<IEnumerable<BatchResponseDto>> GetBatchesByProductIdAsync(Guid productId)
    {
        List<Batch>? batches = await _context.Batches
            .Include(b => b.Product)
            .Where(b => b.ProductId == productId)
            .ToListAsync();

        if (!batches.Any())
        {
            throw new KeyNotFoundException("There is no Batches of a product witch the informed Id");
        }

        return batches.Select(MapToDto).ToList();
    }
    



    public async Task UpdateBatchAsync(Guid id, UpdateBatchDto dto)
    {
        var batch = await _context.Batches
            .Include(b => b.Product)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (batch == null)
            throw new KeyNotFoundException($"Lote com ID {id} não encontrado.");

        var product = await _context.Products
            .FindAsync(dto.ProductId);
        if (product == null)
            throw new KeyNotFoundException($"Produto com ID {dto.ProductId} não encontrado.");

        batch.Registration = dto.Registration;
        batch.FabricationDate = dto.FabricationDate;
        batch.ValidityDate = dto.ValidityDate;
        batch.ProductId = dto.ProductId;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteBatchAsync(Guid id)
    {
        var batch = await _context.Batches
            .Include(b => b.Product)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (batch == null)
            throw new KeyNotFoundException($"Lote com ID {id} não encontrado.");

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
            IsExpired = batch.IsExpired,
            ProductId = batch.ProductId,
            ProductName = batch.Product != null ? batch.Product.Name : string.Empty
        };
    }
}