using System;

namespace Red_Social_Voluntarios.Models
{
    public class Comunidad
    {
        public int Id { get; set; }  // Identificador único de la publicación
        public string Titulo { get; set; }  // Título de la publicación
        public string Contenido { get; set; }  // Texto de la publicación
        public DateTime FechaPublicacion { get; set; } = DateTime.Now;  // Fecha de creación automática
        public int UsuarioId { get; set; }  // Relación con el usuario que creó la publicación
        public Usuario Usuario { get; set; }  // Propiedad de navegación para el usuario
    }
}
