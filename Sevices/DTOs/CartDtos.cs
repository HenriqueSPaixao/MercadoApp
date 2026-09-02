using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Sevices.DTOs
{
    public interface ICartServices
    {
        //task é promessa que será implementado]
        Task<CartResponseDto> CreateCartAsync();
        Task<CartResponseDto> GetCartByIdAsync(int cartId);
        Task AddItemAsync(int cartId, AddItemDto dto);
        Task RemoveItemAsync(int cartId, int productId);
        Task CloseCartAsync(int cartId);
    }
}
