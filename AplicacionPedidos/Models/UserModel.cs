using System.ComponentModel.DataAnnotations;

namespace AplicacionPedidos.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(100)]
        public string Password { get; set; }

        [Required]
        public string Rol { get; set; }
    }
}
