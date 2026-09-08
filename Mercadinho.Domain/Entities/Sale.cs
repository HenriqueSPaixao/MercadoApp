using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Domain.Entities
{
    public class Sale
    {
        Guid SaleId { get; set; }
        public DateTime SaleDate { get; set; }
        public Guid CartId { get; set; }
        public Cart? Cart { get; set; }
        public Sale(Guid cartId) 
        {
            SaleId = Guid.NewGuid();
            CartId = cartId;
            SaleDate = DateTime.UtcNow;
        }
    }
}
