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
        public DateTime ValidityDate { get; set; }
        public int ProductId { get; set; } 
        public Product Product { get; set; } = null!;

        public Batch()
        {
            Id = Guid.NewGuid();
        
        }

        public Batch(string registration, DateTime fabricationDate, DateTime ValidityDate, )
        {
            Id = Guid.NewGuid();

        }


    }
}