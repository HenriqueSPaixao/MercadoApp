using System;
using System.Collections.Generic;
using System.Linq;

namespace Market.Domain.Entities;

public class Cart
{
    public Guid CartId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsClosed { get; private set; }
    public decimal TotalAmount { get; private set; }
    public List<CartItem> Items { get; private set; } = new List<CartItem>();

    public Cart()
    {
        CartId = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        IsClosed = false;
        TotalAmount = 0;
    }
    public void AddItem(CartItem item)
    {
        Items.Add(item);
        TotalAmount += (item.UnitPrice * item.Quantity);
    }

    public void RemoveItem(CartItem item)
    {
        Items.Remove(item);
        TotalAmount -= (item.UnitPrice * item.Quantity);
    }

    public void ClearCart()
    {
        Items.Clear();
        TotalAmount = 0;
    }
    public void CreatedAtChange(DateTime createdAt)
    {
        CreatedAt = createdAt;
    }
    public void CloseCart()
    {
        IsClosed = true;
    }

}