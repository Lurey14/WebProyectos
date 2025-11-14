using System.ComponentModel.DataAnnotations;

namespace ExamenWeb.Models.DTOs
{
    public class ProductoDto
    {
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
    public class RegisterProductoDto
    {
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

    public class UpdateProductoDto
    {
        public int IdProducto { get; set; }
        [Required, MaxLength(100)]
        public string? NombreProducto { get; set; }
        [Required, MaxLength(100)]
        public string? DescripcionCorta { get; set; }
        [Required]
        public string? Precio { get; set; }
        [Required]
        public string? Stock { get; set; }
        [Required]
        public ICollection<Categoria> IdCategoria { get; set; }
    }

    public class DeleteProductoDto
    {
        [Required]
        public int IdProducto { get; set; }
    }
}
