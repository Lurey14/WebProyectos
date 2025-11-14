using System.ComponentModel.DataAnnotations;

namespace ExamenWeb.Models.DTOs
{
    public class CategoriaDto
    {
        [Required, MaxLength(100)]
        public string NombreCategoria { get; set; } = string.Empty;
        [Required, MaxLength(200)]
        public string Descripcion { get; set; } = string.Empty;
    }
    public class RegisterCategoriaDto
    {
        [Required, MaxLength(100)]
        public string NombreCategoria { get; set; } = string.Empty;
        [Required, MaxLength(200)]
        public string Descripcion { get; set; } = string.Empty;
    }

    public class UpdateCategoriaDto
    {
        [Required]
        public int IdCategoria { get; set; }
        [Required, MaxLength(100)]
        public string? NombreCategoria { get; set; }
        [Required, MaxLength(200)]
        public string? Descripcion { get; set; }
    }

    public class DeleteCategoriaDto
    {
        [Required]
        public int IdCategoria { get; set; }
    }
}
