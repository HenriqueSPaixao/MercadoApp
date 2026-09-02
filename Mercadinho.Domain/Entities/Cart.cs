using System;
using System.Collections.Generic;
using System.Linq;

namespace Market.Domain.Entities;

public class Cart
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsClosed { get; private set; } = false;

    // Relacionamento 1:N com CartItem (propriedade protegida)
    public List<CartItem> Items { get; private set; } = new List<CartItem>();

    // Propriedades calculadas automaticamente a partir dos itens
    public int TotalQuantity => Items.Sum(item => item.Quantity);
    public decimal TotalAmount => Items.Sum(item => item.Subtotal);

    public Cart() { }

    // Métodos de negócio (regras centralizadas na raiz de agregação)
    public void AddItem(Product product, int quantity)
    {
        if (IsClosed)
            throw new InvalidOperationException("Não é possível adicionar itens a um carrinho finalizado.");

        if (product == null)
            throw new ArgumentNullException(nameof(product));

        var existingItem = Items.FirstOrDefault(i => i.ProductId == product.Id);

        if (existingItem != null)
        {
            existingItem.IncreaseQuantity(quantity);
        }
        else
        {
            // O CartItem congela o preço de venda s do produto
            Items.Add(new CartItem(product.Id, product.SellingPrice, quantity));
        }
    }

    public void RemoveItem(int productId)
    {
        if (IsClosed)
            throw new InvalidOperationException("Não é possível alterar um carrinho finalizado.");

        var item = Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            Items.Remove(item);
        }
    }

    public void CloseCart()
    {
        if (!Items.Any())
            throw new InvalidOperationException("Não é possível finalizar um carrinho sem itens.");

        IsClosed = true;
    }
}