using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;


        // definir o tipo de relacionamento de uma categoria para muitos produtos
        public ICollection<Product> Products { get; set; } = new List<Product>(); // crio uma lista de produtos para serem adicionados aqui

    }
}
