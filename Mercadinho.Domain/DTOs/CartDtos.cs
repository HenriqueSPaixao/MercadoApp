using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Market.Domain.Entities;
namespace Market.Domain.DTOs;


public class CartResponseDto
{
    public Guid CartId { get; set; }
    public bool IsClosed { get; set; }
    public decimal TotalAmount { get; set; }
    public List<CartItemResponseDto> Items { get; set; } = new List<CartItemResponseDto>();
}

public class CartItemResponseDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal { get; set; }
}
