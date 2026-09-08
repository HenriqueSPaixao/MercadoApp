using System;
using System.ComponentModel.DataAnnotations;

namespace Market.Domain.DTOs
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode ultrapassar 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "O código de barras é obrigatório.")]
        [StringLength(50, ErrorMessage = "O código de barras não pode ultrapassar 50 caracteres.")]
        public string Barcode { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "O preço de custo deve ser maior que zero.")]
        public decimal CostPrice { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "O preço de venda deve ser maior que zero.")]
        public decimal SellingPrice { get; set; }

        [Required(ErrorMessage = "O Id do setor é obrigatório.")]
        public Guid SectorId { get; set; }
    }

    public class UpdateProductDto
    {
        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode ultrapassar 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "O código de barras é obrigatório.")]
        public string Barcode { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "O preço de custo deve ser maior que zero.")]
        public decimal CostPrice { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "O preço de venda deve ser maior que zero.")]
        public decimal SellingPrice { get; set; }

        [Required(ErrorMessage = "O Id do setor é obrigatório.")]
        public Guid SectorId { get; set; }
    }

    public class ProductResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int Quantity { get; set; }
        public int QuantityByBatch { get; set; }
        public bool Active { get; set; }
        public Guid SectorId { get; set; }
        public string SectorName { get; set; } = string.Empty;
    }
}