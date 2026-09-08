using Microsoft.AspNetCore.Mvc;
using Market.Domain.Interfaces;
using Market.Domain.Entities;
using Market.Domain.Implementations;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // URL: http://localhost:porta/api/cart
    public class CartController : ControllerBase
    {
        private readonly ICartServices _cartServices;

        public CartController(ICartServices cartServices)
        {
            _cartServices = cartServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart()
        {
            var result = await _cartServices.CreateCartAsync();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCart(Guid id)
        {
            try
            {
                var result = await _cartServices.GetCartByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem([FromBody] CartItem item)
        {
            // O Entity Framework precisa que o Id venha vazio/novo para não dar conflito
            item.CartItemId = Guid.NewGuid();

            await _cartServices.AddItemAsync(item);
            return Ok(new { message = "Item adicionado com sucesso!" });
        }

        [HttpDelete("{cartId:guid}/items/{itemId:guid}")]
        public async Task<IActionResult> RemoveItem(Guid cartId, Guid itemId)
        {
            try
            {
                await _cartServices.RemoveItemAsync(cartId, itemId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPatch("{id:guid}/close")]
        public async Task<IActionResult> CloseCart(Guid id)
        {
            try
            {
                await _cartServices.CloseCartAsync(id);
                return Ok(new { message = "Carrinho fechado com sucesso!" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCart(Guid id)
        {
            try
            {
                await _cartServices.DeleteCartAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}