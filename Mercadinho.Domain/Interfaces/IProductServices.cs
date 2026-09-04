using Market.Domain.DTOs;

namespace Market.Domain.Interfaces;

public interface IProductServices
{
    Task<ProductResponseDto> CreateProductAsync(CreateProductDto dto);
    Task<ProductResponseDto> GetProductByIdAsync(int id);
    Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
    Task<ProductResponseDto> UpdateProductAsync(int id, UpdateProductDto dto);
    Task DeleteProductAsync(int id);
}