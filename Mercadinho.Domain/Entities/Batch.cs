using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Domain.Entities
{
    public class Batch
    {
        public Guid Id { get; set; }
        public string Registration { get; set; } = string.Empty;
        public DateTime FabricationDate { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime ValidityDate { get; set; }
        public bool IsExpired {  get; set; }
        public int ProductBatchQuantity { get; set; }
        public int EntryQuantity { get; set; }
        public Guid ProductId { get; set; } 
        public Product? Product { get; set; }

        public Batch()
        {
            Id = Guid.NewGuid();
            
        }

        public Batch(string registration, DateTime fabricationDate, DateTime validityDate, int entryQuantity, Guid productId)
        {
            Id = Guid.NewGuid();
            Registration = registration;
            FabricationDate = fabricationDate;
            ValidityDate = validityDate;
            ProductBatchQuantity = entryQuantity;
            EntryQuantity = entryQuantity;
            ProductId = productId;
            if (validityDate.Date < DateTime.Now.Date)
            {
                IsExpired = true;
            }
            else
            {
                IsExpired = false;
            }
        }
    }
}