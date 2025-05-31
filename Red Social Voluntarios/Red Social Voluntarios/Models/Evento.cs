using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Red_Social_Voluntarios.Models
{
    public class Evento
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(200)]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(2000)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        [Display(Name = "Fecha de Inicio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es requerida")]
        [Display(Name = "Fecha de Fin")]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "La ubicación es requerida")]
        [StringLength(300)]
        [Display(Name = "Ubicación")]
        public string Ubicacion { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Ciudad")]
        public string? Ciudad { get; set; }

        [Required(ErrorMessage = "El número de voluntarios es requerido")]
        [Display(Name = "Voluntarios Necesarios")]
        public int VoluntariosNecesarios { get; set; }

        [Display(Name = "Voluntarios Inscritos")]
        public int VoluntariosInscritos { get; set; } = 0;

        [StringLength(500)]
        [Display(Name = "Requisitos")]
        public string? Requisitos { get; set; }

        [StringLength(300)]
        [Display(Name = "Habilidades Necesarias")]
        public string? HabilidadesNecesarias { get; set; }

        [StringLength(100)]
        [Display(Name = "Categoría")]
        public string? Categoria { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        [StringLength(255)]
        [Display(Name = "Imagen")]
        public string? ImagenUrl { get; set; }

        // Relaciones
        [Required]
        [ForeignKey("OrganizadorId")]
        public int OrganizadorId { get; set; }
        public virtual Usuario Organizador { get; set; } = null!;

        public virtual ICollection<InscripcionEvento> Inscripciones { get; set; } = new List<InscripcionEvento>();

        // Propiedades calculadas
        [NotMapped]
        public bool EstaCompleto => VoluntariosInscritos >= VoluntariosNecesarios;

        [NotMapped]
        public bool YaTermino => DateTime.Now > FechaFin;

        [NotMapped]
        public bool EstaEnCurso => DateTime.Now >= FechaInicio && DateTime.Now <= FechaFin;

        [NotMapped]
        public int VacantesDisponibles => Math.Max(0, VoluntariosNecesarios - VoluntariosInscritos);
    }

}

