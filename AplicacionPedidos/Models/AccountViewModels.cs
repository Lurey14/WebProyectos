using System.ComponentModel.DataAnnotations;

namespace AplicacionPedidos.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El Email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inv�lido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La Contrase�a es obligatoria")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El Nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres y m�ximo {1}.", MinimumLength = 2)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El Email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inv�lido")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La Contrase�a es obligatoria")]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres y m�ximo {1}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contrase�a")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contrase�a")]
        [Compare("Password", ErrorMessage = "Las contrase�as no coinciden.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El Rol es obligatorio")]
        [Display(Name = "Rol")]
        public string Role { get; set; } = "Cliente";

        public string[] Roles { get; set; } = { "Cliente", "Empleado" };
    }
}