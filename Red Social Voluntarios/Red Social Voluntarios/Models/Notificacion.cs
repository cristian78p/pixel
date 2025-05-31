using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Red_Social_Voluntarios.Models
{
    public class Notificacion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        [Display(Name = "Mensaje")]
        public string Mensaje { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; } = "Info"; // Info, Success, Warning, Error

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Display(Name = "Leída")]
        public bool Leida { get; set; } = false;

        // Relaciones
        [Required]
        [ForeignKey("UsuarioId")]
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; } = null!;
    }
}
