using System.ComponentModel.DataAnnotations;

namespace Red_Social_Voluntarios.Models
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        [Display(Name = "Correo Electrónico")]
        public string Correo { get; set; } = string.Empty;

        [Display(Name = "Nombre de Usuario")]
        [StringLength(50)]
        public string? NombreUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Contrasena", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarContrasena { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de usuario es requerido")]
        [Display(Name = "Tipo de Usuario")]
        public string Tipo { get; set; } = "Voluntario";
    }
}
