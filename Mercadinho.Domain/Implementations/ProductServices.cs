using Market.Domain.Entities;
using Market.Infrastructure.Data;
using Market.Domain.DTOs;
using Market.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Domain.Implementations
{
    public class ProductServices : IProductServices
    {
        private readonly AppDbContext _context;

        public ProductServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductResponseDto> CreateProductAsync(CreateProductDto dto)
        {
            var sector = await _context.Sectors.FindAsync(dto.SectorId);
            if (sector == null)
                throw new KeyNotFoundException($"Setor com ID {dto.SectorId} não encontrado.");

            var product = new Product(dto.Name, dto.Barcode, dto.CostPrice, dto.SellingPrice);

            product.AssignSector(sector);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return MapToDto(product);
        }

        public async Task<ProductResponseDto> GetProductByIdAsync(Guid id)
        {
            var product = await _context.Products
                .Include(p => p.Sector) 
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");

            return MapToDto(product);
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            var products = await _context.Products
                .AsNoTracking()
                .Include(p => p.Sector)
                .ToListAsync();

            return products.Select(MapToDto).ToList();
        }

        public async Task<ProductResponseDto> UpdateProductAsync(Guid id, UpdateProductDto dto)
        {
            var product = await _context.Products
                .Include(p => p.Sector)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");

            // Usa os métodos da Entidade para atualizar os dados (encapsulamento)
            product.UpdateDetails(dto.Name, dto.Barcode);
            product.UpdatePrices(dto.CostPrice, dto.SellingPrice);

            // Se o setor foi alterado no DTO, busca o novo setor e reatribui
            if (product.Sector == null || product.Sector.Id != dto.SectorId)
            {
                var newSector = await _context.Sectors.FindAsync(dto.SectorId);
                if (newSector == null)
                    throw new KeyNotFoundException($"Setor com ID {dto.SectorId} não encontrado.");

                product.AssignSector(newSector);
            }

            await _context.SaveChangesAsync();
            return MapToDto(product);
        }

        public async Task InactivateProductAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");

            product.Inactivate();

            await _context.SaveChangesAsync();
        }

        public async Task ActivateProductAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");

            product.Activate();

            await _context.SaveChangesAsync();
        }

        private static ProductResponseDto MapToDto(Product product)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Barcode = product.Barcode,
                CostPrice = product.CostPrice,
                SellingPrice = product.SellingPrice,
                Quantity = product.Quantity,
                QuantityByBatch = product.QuantityByBatch,
                Active = product.Active,
                SectorId = product.Sector != null ? product.Sector.Id : Guid.Empty,
                SectorName = product.Sector != null ? product.Sector.Name : string.Empty
            };
        }
    }
}