using Market.Domain.DTOs;

namespace Market.Domain.Interfaces;

public interface IBatchServices
{
    void CreateBatchAsync(CreateBatchDto dto);
    Task<BatchResponseDto> GetBatchByRegistrationAsync(Guid BatchId);
    Task<IEnumerable<BatchResponseDto>> GetAllBatchesAsync();
    Task<IEnumerable<BatchResponseDto>> GetBatchesByProductBarcodeAsync(Guid ProductId);
    Task<BatchResponseDto> UpdateBatchAsync(string registration, string productBarcode, UpdateBatchDto dto);
    Task DeleteBatchAsync(int id);
}