using System.ComponentModel.DataAnnotations;

namespace Market.Services.DTOs;

public class CreateCategoryDto
{
    [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome não pode ultrapassar 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(250, ErrorMessage = "A descrição não pode ultrapassar 250 caracteres.")]
    public string Description { get; set; } = string.Empty;
}

public class UpdateCategoryDto
{
    [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome não pode ultrapassar 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(250, ErrorMessage = "A descrição não pode ultrapassar 250 caracteres.")]
    public string Description { get; set; } = string.Empty;
}

public class CategoryResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}