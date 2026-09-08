using Market.Domain.Entities;
using Market.Infrastructure.Data;
using Market.Domain.DTOs;
using Market.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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
        Product? product = await _context.Products
            .FindAsync(dto.ProductId);

        if (product == null)
        {
            throw new KeyNotFoundException($"Produto com ID {dto.ProductId} não encontrado.");
        }
        product.IncreaseQuantity(dto.EntryQuantity);
        Batch batch = new Batch
        (
            dto.Registration,
            dto.FabricationDate,
            dto.ValidityDate,
            dto.EntryQuantity,
            dto.ProductId
        );
        
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

        return MapToResponseDto(batch);
    }

    public async Task<IEnumerable<BatchResponseDto>> GetAllBatchesAsync()
    {
        List<Batch> batches = await _context.Batches
            .AsNoTracking()// Não vai armazenar a busca em cache
            .Include(b => b.Product)
            .ToListAsync();

        return batches.Select(MapToResponseDto);
    }

    public async Task<IEnumerable<BatchResponseDto>> GetBatchesByProductIdAsync(Guid productId)
    {
        List<Batch> batches = await _context.Batches
            .Include(b => b.Product)
            .Where(b => b.ProductId == productId)
            .ToListAsync();

        if (!batches.Any())
        {
            throw new KeyNotFoundException("There is no product batch with the informed ID");
        }

        return batches.Select(MapToResponseDto);
    }



    public async Task UpdateBatchAsync(Guid batchId, UpdateBatchDto dto)
    {
        Batch? batch = await _context.Batches
            .Include(b => b.Product)
            .FirstOrDefaultAsync(b => b.Id == batchId);

        if(batch == null)
        {
            throw new KeyNotFoundException($"The batch id {batchId} was not found");
        }

        batch.Registration = dto.Registration;
        batch.ValidityDate = dto.ValidityDate;
        batch.FabricationDate = dto.FabricationDate;
        batch.EntryQuantity = dto.EntryQuantity;
        batch.ProductId = dto.ProductId;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteBatchAsync(Guid batchId)
    {
        Batch? batch = await _context.Batches
            .Include(b => b.Product)
            .FirstOrDefaultAsync(b => b.Id == batchId);


        if (batch == null)
            throw new KeyNotFoundException($"Lote com ID {batchId} não encontrado.");

        // Desconta do produto a quantidade do lote deletado
        if (batch.Product != null)
            batch.Product.DecreaseQuantity(batch.ProductBatchQuantity);

        _context.Batches.Remove(batch);
        await _context.SaveChangesAsync();
    }

    private static BatchResponseDto MapToResponseDto(Batch batch)
    {
        return new BatchResponseDto
        {
            Id = batch.Id,
            Registration = batch.Registration,
            FabricationDate = batch.FabricationDate,
            ValidityDate = batch.ValidityDate,
            StartDate = batch.StartDate,
            IsExpired = batch.IsExpired,
            EntryQuantity = batch.EntryQuantity,
            ProductBatchQuantity = batch.ProductBatchQuantity,
            ProductName = batch.Product != null ? batch.Product.Name : string.Empty
        };
    }
}