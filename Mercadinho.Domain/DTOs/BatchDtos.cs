using System.ComponentModel.DataAnnotations;

namespace Market.Domain.DTOs;

public class CreateBatchDto
{
    [Required(ErrorMessage = "O registro do lote é obrigatório.")]
    [StringLength(50, ErrorMessage = "O registro não pode ultrapassar 50 caracteres.")]
    public string Registration { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data de fabricação é obrigatória.")]
    public DateTime FabricationDate { get; set; }

    [Required(ErrorMessage = "A data de validade é obrigatória.")]
    public DateTime ValidityDate { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser de pelo menos 1 unidade.")]
    public int Amount { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Informe um ID de produto válido.")]
    public int ProductId { get; set; }
}

public class UpdateBatchDto
{
    [Required(ErrorMessage = "O registro do lote é obrigatório.")] //metadata 
    [StringLength(50, ErrorMessage = "O registro não pode ultrapassar 50 caracteres.")]
    public string Registration { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data de fabricação é obrigatória.")]
    public DateTime FabricationDate { get; set; }

    [Required(ErrorMessage = "A data de validade é obrigatória.")]
    public DateTime ValidityDate { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "A quantidade não pode ser negativa.")]
    public int Amount { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Informe um ID de produto válido.")]
    public int ProductId { get; set; }
}

public class BatchResponseDto
{
    public int Id { get; set; }
    public string Registration { get; set; } = string.Empty;
    public DateTime FabricationDate { get; set; }
    public DateTime ValidityDate { get; set; }
    public int Amount { get; set; }
    public bool IsExpired { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
}