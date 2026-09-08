using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Market.Domain.DTOs;

namespace Market.Domain.Interfaces
{
    public interface IProductServices
    {
        Task<ProductResponseDto> CreateProductAsync(CreateProductDto dto);
        Task<ProductResponseDto> GetProductByIdAsync(Guid id);
        Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
        Task<ProductResponseDto> UpdateProductAsync(Guid id, UpdateProductDto dto);
        Task InactivateProductAsync(Guid id);
        Task ActivateProductAsync(Guid id);
    }
}