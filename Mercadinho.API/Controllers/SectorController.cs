using Microsoft.AspNetCore.Mvc;
using Market.Domain.Interfaces;
using Market.Domain.DTOs;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectorController : ControllerBase
    {
        private readonly ISectorServices _sectorServices;

        public SectorController(ISectorServices sectorServices)
        {
            _sectorServices = sectorServices;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSectorDto dto)
        {
            var result = await _sectorServices.CreateSectorAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _sectorServices.GetSectorByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _sectorServices.GetAllSectorsAsync();
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSectorDto dto)
        {
            try
            {
                var result = await _sectorServices.UpdateSectorAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _sectorServices.DeleteSectorAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // 400: Se tentar apagar setor com produtos dentro
            }
        }
    }
}