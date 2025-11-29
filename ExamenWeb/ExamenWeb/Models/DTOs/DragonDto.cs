using System.ComponentModel.DataAnnotations;

namespace ExamenWeb.Models.DTOs
{
    public class DragonDto
    {
        [Required, MaxLength(100)]
        public string NombreDragon { get; set; } = string.Empty;
        [Required, MaxLength(200)]
        public string Descripcion { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string MadreDeDragones { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string Color { get; set; } = string.Empty;
    }
    public class RegisterDragonDto
    {
        [Required, MaxLength(100)]
        public string NombreDragon { get; set; } = string.Empty;
        [Required, MaxLength(200)]
        public string Descripcion { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string MadreDeDragones { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string Color { get; set; } = string.Empty;
    }

    public class UpdateDragonDto
    {
        public int IdDragon { get; set; }
        [Required, MaxLength(100)]
        public string? NombreDragon { get; set; }
        [Required, MaxLength(200)]
        public string? Descripcion { get; set; }
        [Required, MaxLength(100)]
        public string? MadreDeDragones { get; set; }
        [Required, MaxLength(50)]
        public string? Color { get; set; }
    }

    public class DeleteDragonDto
    {
        [Required]
        public int IdDragon { get; set; }
    }
}
