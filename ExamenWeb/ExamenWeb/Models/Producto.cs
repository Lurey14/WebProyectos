using System.ComponentModel.DataAnnotations;

namespace ExamenWeb.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        [Required, MaxLength(100)]
        public string NombreProducto { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string DescripcionCorta { get; set; } = string.Empty;
        [Required]
        public string Precio { get; set; } = string.Empty;
        [Required]
        public string Stock { get; set; } = string.Empty;
        [Required]
        public ICollection<Categoria> IdCategoria { get; set; }
    }
}
