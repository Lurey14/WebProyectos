using System.ComponentModel.DataAnnotations;

namespace ExamenWeb.Models
{
    public class Proveedor
    {
        public int IdProveedor { get; set; }
        [Required, MaxLength(100)]
        public string RazonSocial { get; set; } = string.Empty;
        [Required]
        public string Contacto { get; set; } = string.Empty;
    }
}
