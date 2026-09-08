using System;
using System.ComponentModel.DataAnnotations;

namespace Market.Domain.DTOs
{
    public class CreateSectorDto
    {
        [Required(ErrorMessage = "O nome do setor é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode ultrapassar 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "A descrição não pode ultrapassar 255 caracteres.")]
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateSectorDto
    {
        [Required(ErrorMessage = "O nome do setor é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode ultrapassar 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "A descrição não pode ultrapassar 255 caracteres.")]
        public string Description { get; set; } = string.Empty;
    }

    public class SectorResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}