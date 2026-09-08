using Market.Domain.DTOs;

namespace Market.Domain.Interfaces;

public interface IBatchServices
{
    Task CreateBatchAsync(CreateBatchDto dto);
    Task<BatchResponseDto> GetBatchByIdAsync(Guid BatchId);
    Task<IEnumerable<BatchResponseDto>> GetAllBatchesAsync();
    Task<IEnumerable<BatchResponseDto>> GetBatchesByProductIdAsync(Guid ProductId);
    Task UpdateBatchAsync(Guid BatchID, UpdateBatchDto dto);
    Task DeleteBatchAsync(Guid BatchId);
}