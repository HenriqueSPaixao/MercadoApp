using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Market.Domain.Entities;
using Market.Domain.DTOs;// Permite enxergar o CartResponseDto e AddItemDto

namespace Market.Domain.Interfaces
{
    public interface ICartServices
    {
    Task<CartResponseDto> CreateCartAsync();
    Task<CartResponseDto> GetCartByIdAsync(Guid cartId);
    Task AddItemAsync(CartItem cartId);

    Task ClearCartAsync(Guid cartId);
    Task DeleteCartAsync(Guid cartId);
    Task RemoveItemAsync(Guid cartId, Guid itemId);
    Task CloseCartAsync(Guid cartId);

}
}
