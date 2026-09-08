using System;
using System.Collections.Generic;

namespace Market.Domain.Entities
{
    public class Sector
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;

        // Usamos ICollection que é o padrão mais recomendado para relacionamentos no EF Core
        public ICollection<Product> Products { get; private set; } = new List<Product>();

        // Construtor vazio exigido pelo Entity Framework Core
        public Sector()
        {
            Id = Guid.NewGuid();
        }

        // Construtor rico para quando você for criar um setor novo
        public Sector(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome do setor é obrigatório.", nameof(name));

            Id = Guid.NewGuid();
            Name = name;
            Description = description;
        }

        // Método de negócio para permitir edição segura pelo Service
        public void UpdateDetails(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome do setor é obrigatório.", nameof(name));

            Name = name;
            Description = description;
        }
    }
}