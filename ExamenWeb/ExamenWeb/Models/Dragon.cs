using System.ComponentModel.DataAnnotations;

namespace ExamenWeb.Models
{
    public class Dragon
    {
        [Key]
        public int IdDragon { get; set; }
        [Required, MaxLength(100)]
        public string NombreDragon { get; set; } = string.Empty;
        [Required, MaxLength(200)]
        public string Descripcion { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string MadreDeDragones { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string Color { get; set; } = string.Empty;
    }
}
