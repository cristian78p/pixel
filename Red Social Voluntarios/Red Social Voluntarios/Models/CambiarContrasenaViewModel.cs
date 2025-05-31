using System.ComponentModel.DataAnnotations;

namespace Red_Social_Voluntarios.Models
{
    public class CambiarContrasenaViewModel
    {
        [Required(ErrorMessage = "La contraseña actual es requerida")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña Actual")]
        public string ContrasenaActual { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nueva contraseña es requerida")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Contraseña")]
        public string NuevaContrasena { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirma la nueva contraseña")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Nueva Contraseña")]
        [Compare("NuevaContrasena", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarNuevaContrasena { get; set; } = string.Empty;
    }
}
