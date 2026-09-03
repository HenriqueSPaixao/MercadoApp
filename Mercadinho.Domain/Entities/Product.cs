using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int Quantity { get; set; }
        public bool Active { get; set; }

        public CartItem? Item { get; set; }
        public List<Batch> ProductBatches = new List<Batch>();

        public Product()
        {
            Id = Guid.NewGuid();
        }

        public Product(string name, string barcode, decimal costPrice, decimal sellingPrice, int quantity)
        {
            Id = Guid.NewGuid();
            Name = name;
            Barcode = barcode;
            CostPrice = costPrice;
            SellingPrice = sellingPrice;
            Active = false;
            Quantity = quantity;
        }
    }
}
