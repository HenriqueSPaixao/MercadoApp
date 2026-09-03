using System;
using System.Collections.Generic;
using System.Linq;

namespace Market.Domain.Entities;

public class Cart
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsClosed { get; private set; }

    public List<CartItem>? Items { get; set; }

    public Cart()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        IsClosed = false;
    }

    public void IsClosedChange()
    {
        IsClosed = true;
    }
}