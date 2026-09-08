using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace Market.Domain.DTOs;

public class CreateBatchDto
{
    [Required(ErrorMessage = "The Registration is a required parameter")]
    [StringLength(50, ErrorMessage = "The registration can not be larger than 50 Characters")]
    public string Registration { get; set; } = string.Empty;

    [Required(ErrorMessage = "The fabrication date is a Required parameter")]
    public DateTime FabricationDate { get; set; }
    [Required (ErrorMessage = "The EntryQuantity is a Required parameter")]
    public int EntryQuantity { get; set; }

    [Required(ErrorMessage = "The validity date is a Required parameter.")]
    public DateTime ValidityDate { get; set; }
    [Required(ErrorMessage = "The Product´s Id is a required paremeter")]
    public Guid ProductId { get; set; }

}


public class UpdateBatchDto
{
    [Required(ErrorMessage = "The Batch registration is required to update")]
    [StringLength(50, ErrorMessage = "The Batch registration can not bypass 50 chars")]
    public string Registration { get; set; } = string.Empty;
    [Required(ErrorMessage = "The Batch validity date is required to update")]
    public DateTime ValidityDate { get; set; }
    [Required(ErrorMessage = "The Batch fabrication date is required to update")]
    public DateTime FabricationDate { get; set; }
    [Required(ErrorMessage = "The Product Batch Quantity is required to update")]

    public int ProductBatchQuantity { get; set; }
    [Required(ErrorMessage = "The Batch Entry Quantity is required to update")]
    public int EntryQuantity { get; set; }

    [Required(ErrorMessage = "The Product´s ID is required to update")]

    public Guid ProductId { get; set; }
}


public class BatchResponseDto
{
    public Guid Id { get; set; }
    public string Registration { get; set; } = string.Empty;
    public DateTime FabricationDate { get; set; }
    public DateTime ValidityDate { get; set; }
    public DateTime StartDate { get; set; }
    public bool IsExpired { get; set; }
    public int ProductBatchQuantity { get; set; }
    public int EntryQuantity { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
}