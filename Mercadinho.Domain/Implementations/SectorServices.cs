using Market.Domain.Entities;
using Market.Infrastructure.Data;
using Market.Domain.DTOs;
using Market.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Market.Domain.Implementations
{
    public class SectorServices : ISectorServices
    {
        private readonly AppDbContext _context;

        public SectorServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SectorResponseDto> CreateSectorAsync(CreateSectorDto dto)
        {
            // Usa o construtor rico da entidade
            var sector = new Sector(dto.Name, dto.Description);

            _context.Sectors.Add(sector);
            await _context.SaveChangesAsync();

            return MapToDto(sector);
        }

        public async Task<SectorResponseDto> GetSectorByIdAsync(Guid id)
        {
            var sector = await _context.Sectors.FindAsync(id);

            if (sector == null)
                throw new KeyNotFoundException($"Setor com ID {id} não encontrado.");

            return MapToDto(sector);
        }

        public async Task<IEnumerable<SectorResponseDto>> GetAllSectorsAsync()
        {
            var sectors = await _context.Sectors
                .AsNoTracking()
                .ToListAsync();

            return sectors.Select(MapToDto).ToList();
        }

        public async Task<SectorResponseDto> UpdateSectorAsync(Guid id, UpdateSectorDto dto)
        {
            var sector = await _context.Sectors.FindAsync(id);

            if (sector == null)
                throw new KeyNotFoundException($"Setor com ID {id} não encontrado.");

            // Chama o método de negócio para atualizar os dados mantendo os setters privados
            sector.UpdateDetails(dto.Name, dto.Description);

            await _context.SaveChangesAsync();

            return MapToDto(sector);
        }

        public async Task DeleteSectorAsync(Guid id)
        {
            var sector = await _context.Sectors.FindAsync(id);

            if (sector == null)
                throw new KeyNotFoundException($"Setor com ID {id} não encontrado.");

            // Verifica se existem produtos amarrados a este setor antes de apagar
            // Necessário carregar os produtos ou fazer um AnyAsync() para evitar erro de Foreign Key no banco
            bool hasProducts = await _context.Products.AnyAsync(p => p.Sector.Id == id);

            if (hasProducts)
                throw new InvalidOperationException("Não é possível excluir um setor que possui produtos vinculados.");

            _context.Sectors.Remove(sector);
            await _context.SaveChangesAsync();
        }

        private static SectorResponseDto MapToDto(Sector sector)
        {
            return new SectorResponseDto
            {
                Id = sector.Id,
                Name = sector.Name,
                Description = sector.Description
            };
        }
    }
}