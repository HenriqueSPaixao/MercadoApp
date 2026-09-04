using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Market.Domain.Entities
{
    public class Sector
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Product>? Products { get; set; } = new List<Product>(); // crio uma lista de produtos para serem adicionados aqui
        //new aloca na memória
        public Sector(string name, string description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
        }

    }
}
