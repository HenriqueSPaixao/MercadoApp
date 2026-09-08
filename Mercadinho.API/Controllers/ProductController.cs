using Microsoft.AspNetCore.Mvc;
using Market.Domain.Interfaces;
using Market.Domain.DTOs;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // A URL será: http://localhost:porta/api/product
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            try
            {
                ProductResponseDto? result = await _productServices.CreateProductAsync(dto);
                // Retorna 201 Created
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 404: Setor não encontrado
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // 400: Erros de validação
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _productServices.GetProductByIdAsync(id);
                return Ok(result); // 200 OK
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 404
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productServices.GetAllProductsAsync();
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto dto)
        {
            try
            {
                var result = await _productServices.UpdateProductAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPatch("{id:guid}/inactivate")]
        public async Task<IActionResult> Inactivate(Guid id)
        {
            try
            {
                await _productServices.InactivateProductAsync(id);
                return NoContent(); // 204 No Content (Padrão para ações que não retornam corpo)
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}