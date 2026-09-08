using Market.Domain.DTOs;
using Market.Domain.Entities;
using Market.Domain.Interfaces;
using Market.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Market.Domain.Implementations
{
    public class CartServices : ICartServices
    {
        private readonly AppDbContext _context;
        public CartServices(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CartResponseDto> CreateCartAsync()
        {
            Cart? cart = new Cart();
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();

            return new CartResponseDto
            {
                CartId = cart.CartId,
                IsClosed = cart.IsClosed,
                TotalAmount = cart.TotalAmount,
            };
        }

        public async Task AddItemAsync(CartItem cartItem)
        {
            Cart? cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.IsClosed == false);
            
            if(cart == null)
            {
                cart = new Cart();

                await _context.Carts.AddAsync(cart);
            }
            
            cart.AddItem(cartItem);

            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync(Guid cartId)
        {
            Cart? cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.CartId == cartId);

            if (cart == null)
            {
                throw new KeyNotFoundException($"There is no cart with the Id {cartId}.");
            }
            cart.Items.Clear();
            cart.ClearCart();

            await _context.SaveChangesAsync();

        }

        public async Task CloseCartAsync(Guid cartId)
        {
            Cart? cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.CartId == cartId);
            if (cart == null)
            {
                throw new KeyNotFoundException($"There is no cart with the Id {cartId}.");

            }
            cart.CloseCart();
            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(Guid cartId, Guid itemId)
        {
            Cart? cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CartId == cartId);

            if (cart == null)
            {
                throw new KeyNotFoundException($"There is no cart with the Id {cartId}.");

            }
            CartItem? itemToRemove = cart.Items
                .FirstOrDefault(i => i.CartItemId == itemId);

            if (itemToRemove == null)
            {
                throw new KeyNotFoundException($"There is no item with the Id {itemId} in this cart.");
            }

            cart.RemoveItem(itemToRemove);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartAsync(Guid cartId)
        {
            Cart? cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.CartId == cartId);
            if (cart == null)
            {
                throw new KeyNotFoundException($"There is no cart with the Id {cartId}.");
            }
            await ClearCartAsync(cartId);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }

        public async Task<CartResponseDto> GetCartByIdAsync(Guid cartId)
        {
            Cart? cart = await _context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.CartId == cartId);

            if (cart == null)
            {
                throw new KeyNotFoundException($"There is no cart with the Id {cartId}.");
            }

            return new CartResponseDto
            {
                CartId = cartId,
                IsClosed = cart.IsClosed,
                TotalAmount = cart.TotalAmount,
                Items = cart.Items.Select(item => new CartItemResponseDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product != null ? item.Product.Name : "N/A",
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    Subtotal = item.UnitPrice * item.Quantity
                }).ToList()
            };
        }


    }
}