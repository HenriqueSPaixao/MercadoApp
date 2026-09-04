using Market.Domain.Entities;
using Market.Infrastructure.Data;
using Market.Domain.DTOs;
using Market.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Market.Domain.Implementations;

public class CategoryServices : ICategoryServices
{
    private readonly AppDbContext _context;

    public CategoryServices(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryDto dto)
    {
        var category = new Sector
        {
            Name = dto.Name,
            Description = dto.Description
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return MapToDto(category);
    }

    public async Task<CategoryResponseDto> GetCategoryByIdAsync(int id)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            throw new KeyNotFoundException($"Categoria com ID {id} não encontrada.");

        return MapToDto(category);
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync()
    {
        var categories = await _context.Categories
            .AsNoTracking()
            .ToListAsync();

        return categories.Select(MapToDto).ToList();
    }

    public async Task<CategoryResponseDto> UpdateCategoryAsync(int id, UpdateCategoryDto dto)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            throw new KeyNotFoundException($"Categoria com ID {id} não encontrada.");

        category.Name = dto.Name;
        category.Description = dto.Description;

        await _context.SaveChangesAsync();

        return MapToDto(category);
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            throw new KeyNotFoundException($"Categoria com ID {id} não encontrada.");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

    private static CategoryResponseDto MapToDto(Sector category)
    {
        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }
}