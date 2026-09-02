using System.ComponentModel.DataAnnotations;

namespace Market.Services.DTOs;

// DTO para receber dados na criação de um produto (POST)
public class CreateProductDto
{
    [Required(ErrorMessage = "O nome do produto é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome não pode ultrapassar 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "O preço de venda deve ser maior que zero.")]
    public decimal SellingPrice { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo.")]
    public int Stock { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Informe um Id de categoria válido.")]
    public int CategoryId { get; set; }
}

// DTO para receber dados na atualização de um produto (PUT)
public class UpdateProductDto
{
    [Required(ErrorMessage = "O nome do produto é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome não pode ultrapassar 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "O preço de venda deve ser maior que zero.")]
    public decimal SellingPrice { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo.")]
    public int Stock { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Informe um Id de categoria válido.")]
    public int CategoryId { get; set; }
}

// DTO devolvido nas consultas (GET, retorno de POST e PUT)
public class ProductResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal SellingPrice { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}