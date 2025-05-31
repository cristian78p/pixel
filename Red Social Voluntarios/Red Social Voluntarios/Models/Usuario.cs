 using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    namespace Red_Social_Voluntarios.Models
    {
        public class Usuario
        {
            [Key]
            public int Id { get; set; }

            [Required(ErrorMessage = "El correo electrónico es requerido")]
            [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
            [Display(Name = "Correo Electrónico")]
            [StringLength(100)]
            public string Correo { get; set; } = string.Empty;

            [Display(Name = "Nombre de Usuario")]
            [StringLength(50)]
            public string? NombreUsuario { get; set; }

            [Required(ErrorMessage = "La contraseña es requerida")]
            [StringLength(255, MinimumLength = 6)]
            [Display(Name = "Contraseña")]
            [JsonIgnore]
            public string Contrasena { get; set; } = string.Empty;

            [Required(ErrorMessage = "El tipo de usuario es requerido")]
            [Display(Name = "Tipo de Usuario")]
            [StringLength(20)]
            public string Tipo { get; set; } = "Voluntario";

            [Display(Name = "Fecha de Registro")]
            public DateTime FechaRegistro { get; set; } = DateTime.Now;

            [Display(Name = "Último Acceso")]
            public DateTime? UltimoAcceso { get; set; }

            [Display(Name = "Activo")]
            public bool Activo { get; set; } = true;

            [Display(Name = "Email Verificado")]
            public bool EmailVerificado { get; set; } = false;

            // Propiedades del perfil
            [Display(Name = "Nombre Completo")]
            [StringLength(100)]
            public string? NombreCompleto { get; set; }

            [Display(Name = "Teléfono")]
            [Phone]
            [StringLength(15)]
            public string? Telefono { get; set; }

            [Display(Name = "Dirección")]
            [StringLength(200)]
            public string? Direccion { get; set; }

            [Display(Name = "Ciudad")]
            [StringLength(50)]
            public string? Ciudad { get; set; }

            [Display(Name = "País")]
            [StringLength(50)]
            public string? Pais { get; set; } = "Colombia";

            [Display(Name = "Fecha de Nacimiento")]
            [DataType(DataType.Date)]
            public DateTime? FechaNacimiento { get; set; }

            [Display(Name = "Biografía")]
            [StringLength(500)]
            public string? Biografia { get; set; }

            [Display(Name = "Foto de Perfil")]
            [StringLength(255)]
            public string? FotoPerfil { get; set; }

            [Display(Name = "Habilidades")]
            [StringLength(300)]
            public string? Habilidades { get; set; }

            [Display(Name = "Disponibilidad")]
            [StringLength(100)]
            public string? DisponibilidadHorario { get; set; }

            [Display(Name = "Intereses de Voluntariado")]
            [StringLength(300)]
            public string? InteresesVoluntariado { get; set; }

            // Propiedades para organizaciones
            [Display(Name = "Nombre de la Organización")]
            [StringLength(100)]
            public string? NombreOrganizacion { get; set; }

            [Display(Name = "Descripción de la Organización")]
            [StringLength(1000)]
            public string? DescripcionOrganizacion { get; set; }

            [Display(Name = "Sitio Web")]
            [StringLength(255)]
            public string? SitioWeb { get; set; }

            // Relaciones
            public virtual ICollection<Evento> EventosCreados { get; set; } = new List<Evento>();
            public virtual ICollection<InscripcionEvento> Inscripciones { get; set; } = new List<InscripcionEvento>();
            public virtual ICollection<Comunidad> Publicaciones { get; set; } = new List<Comunidad>();
            public virtual ICollection<Mensaje> MensajesEnviados { get; set; } = new List<Mensaje>();
            public virtual ICollection<Mensaje> MensajesRecibidos { get; set; } = new List<Mensaje>();

            // Propiedades calculadas
            [NotMapped]
            public int? Edad
            {
                get
                {
                    if (FechaNacimiento.HasValue)
                    {
                        var hoy = DateTime.Today;
                        var edad = hoy.Year - FechaNacimiento.Value.Year;
                        if (FechaNacimiento.Value.Date > hoy.AddYears(-edad))
                            edad--;
                        return edad;
                    }
                    return null;
                }
            }

            [NotMapped]
            public string NombreParaMostrar =>
                !string.IsNullOrEmpty(NombreCompleto) ? NombreCompleto :
                (!string.IsNullOrEmpty(NombreOrganizacion) ? NombreOrganizacion :
                (!string.IsNullOrEmpty(NombreUsuario) ? NombreUsuario : Correo));

            // Métodos
            public bool EsVoluntario() => Tipo.Equals("Voluntario", StringComparison.OrdinalIgnoreCase);
            public bool EsOrganizacion() => Tipo.Equals("Organizacion", StringComparison.OrdinalIgnoreCase);
            public bool EsAdministrador() => Tipo.Equals("Administrador", StringComparison.OrdinalIgnoreCase);

            public void ActualizarUltimoAcceso()
            {
                UltimoAcceso = DateTime.Now;
            }
        }
    }