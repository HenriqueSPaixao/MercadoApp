using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Domain.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int Quantity {  get; set; }
        public List<Product> CartItem = new List<Product>();

        public decimal Subtotal { get; set; }

    }
}
