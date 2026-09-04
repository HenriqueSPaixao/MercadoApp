using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Domain.DTOs;

public class AddItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class CartResponseDto
{
    public int Id { get; set; }
    public bool IsClosed { get; set; }
    public decimal TotalAmount { get; set; }
    public int TotalQuantity { get; set; }
    public List<CartItemResponseDto> Items { get; set; } = new List<CartItemResponseDto>();
}

public class CartItemResponseDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal { get; set; }
}
