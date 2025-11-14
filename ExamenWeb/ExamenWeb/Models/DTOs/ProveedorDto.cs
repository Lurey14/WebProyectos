using System.ComponentModel.DataAnnotations;

namespace ExamenWeb.Models.DTOs
{
    public class ProveedorDto
    {
        [Required, MaxLength(100)]
        public string RazonSocial { get; set; } = string.Empty;
        [Required]
        public string Contacto { get; set; } = string.Empty;
    }
    public class RegisterProveedorDto
    {
        [Required, MaxLength(100)]
        public string RazonSocial { get; set; } = string.Empty;
        [Required]
        public string Contacto { get; set; } = string.Empty;
    }

    public class UpdateProveedorDto
    {
        public int IdProveedor { get; set; }
        [Required, MaxLength(100)]
        public string? RazonSocial { get; set; }
        [Required]
        public string? Contacto { get; set; }
    }

    public class DeleteProveedorDto
    {
        [Required]
        public int IdProveedor { get; set; }
    }
}
