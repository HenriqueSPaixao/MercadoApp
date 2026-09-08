using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Market.Domain.DTOs;

namespace Market.Domain.Interfaces
{
    public interface ISectorServices
    {
        Task<SectorResponseDto> CreateSectorAsync(CreateSectorDto dto);
        Task<SectorResponseDto> GetSectorByIdAsync(Guid id);
        Task<IEnumerable<SectorResponseDto>> GetAllSectorsAsync();
        Task<SectorResponseDto> UpdateSectorAsync(Guid id, UpdateSectorDto dto);
        Task DeleteSectorAsync(Guid id);
    }
}