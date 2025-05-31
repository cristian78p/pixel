using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Red_Social_Voluntarios.Models
{
    public class InscripcionEvento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("EventoId")]
        public int EventoId { get; set; }
        public virtual Evento Evento { get; set; } = null!;

        [Required]
        [ForeignKey("VoluntarioId")]
        public int VoluntarioId { get; set; }
        public virtual Usuario Voluntario { get; set; } = null!;

        [Display(Name = "Fecha de Inscripción")]
        public DateTime FechaInscripcion { get; set; } = DateTime.Now;

        [Display(Name = "Estado")]
        [StringLength(20)]
        public string Estado { get; set; } = "Pendiente"; // Pendiente, Confirmado, Cancelado

        [StringLength(500)]
        [Display(Name = "Comentarios")]
        public string? Comentarios { get; set; }

        [Display(Name = "Confirmado por Organizador")]
        public bool ConfirmadoPorOrganizador { get; set; } = false;

        [Display(Name = "Asistió")]
        public bool Asistio { get; set; } = false;
    }
}

