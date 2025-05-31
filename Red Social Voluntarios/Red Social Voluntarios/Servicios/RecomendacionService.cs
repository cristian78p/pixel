using Red_Social_Voluntarios.Data;
using Red_Social_Voluntarios.Models;
using Microsoft.EntityFrameworkCore;

namespace Red_Social_Voluntarios.Services
{
    public class RecomendacionService
    {
        private readonly ApplicationDbContext _context;

        public RecomendacionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Evento>> ObtenerEventosRecomendadosAsync(int usuarioId)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null || !usuario.EsVoluntario())
                return new List<Evento>();

            var eventosRecomendados = await _context.Eventos
                .Include(e => e.Organizador)
                .Where(e => e.Activo && e.FechaInicio > DateTime.Now)
                .Where(e => !e.Inscripciones.Any(i => i.VoluntarioId == usuarioId))
                .Take(5)
                .ToListAsync();

            return eventosRecomendados;
        }

        public async Task<List<Usuario>> ObtenerVoluntariosRecomendadosAsync(int organizacionId)
        {
            var organizacion = await _context.Usuarios.FindAsync(organizacionId);
            if (organizacion == null || !organizacion.EsOrganizacion())
                return new List<Usuario>();

            var voluntarios = await _context.Usuarios
                .Where(u => u.EsVoluntario() && u.Activo)
                .Take(10)
                .ToListAsync();

            return voluntarios;
        }
    }
}