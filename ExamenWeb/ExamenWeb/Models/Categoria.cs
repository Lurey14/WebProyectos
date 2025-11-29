using System.ComponentModel.DataAnnotations;

namespace ExamenWeb.Models
{
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }
        [Required, MaxLength(100)]
        public string NombreCategoria { get; set; } = string.Empty;
        [Required, MaxLength(200)]
        public string Descripcion { get; set; } = string.Empty;
    }
}
