using Market.Domain.DTOs;

namespace Market.Domain.Interfaces;

public interface ICategoryServices
{
    Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryDto dto);
    Task<CategoryResponseDto> GetCategoryByIdAsync(int id);
    Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync();
    Task<CategoryResponseDto> UpdateCategoryAsync(int id, UpdateCategoryDto dto);
    Task DeleteCategoryAsync(int id);
}