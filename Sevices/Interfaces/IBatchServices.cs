using Market.Services.DTOs;

namespace Market.Sevices.Interfaces;

public interface IBatchServices
{
    Task<BatchResponseDto> CreateBatchAsync(CreateBatchDto dto);
    Task<BatchResponseDto> GetBatchByIdAsync(int id);
    Task<IEnumerable<BatchResponseDto>> GetAllBatchesAsync();
    Task<IEnumerable<BatchResponseDto>> GetBatchesByProductIdAsync(int productId);
    Task<BatchResponseDto> UpdateBatchAsync(int id, UpdateBatchDto dto);
    Task DeleteBatchAsync(int id);
}