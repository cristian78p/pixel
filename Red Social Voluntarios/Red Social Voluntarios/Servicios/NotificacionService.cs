using Red_Social_Voluntarios.Data;
using Red_Social_Voluntarios.Models;
using Microsoft.EntityFrameworkCore;

namespace Red_Social_Voluntarios.Services
{
    public class NotificacionService
    {
        private readonly ApplicationDbContext _context;

        public NotificacionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CrearNotificacionAsync(int usuarioId, string titulo, string mensaje, string tipo = "Info")
        {
            var notificacion = new Notificacion
            {
                UsuarioId = usuarioId,
                Titulo = titulo,
                Mensaje = mensaje,
                Tipo = tipo
            };

            _context.Notificaciones.Add(notificacion);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notificacion>> ObtenerNotificacionesUsuarioAsync(int usuarioId)
        {
            return await _context.Notificaciones
                .Where(n => n.UsuarioId == usuarioId)
                .OrderByDescending(n => n.FechaCreacion)
                .Take(10)
                .ToListAsync();
        }

        public async Task MarcarComoLeidaAsync(int notificacionId)
        {
            var notificacion = await _context.Notificaciones.FindAsync(notificacionId);
            if (notificacion != null)
            {
                notificacion.Leida = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> ContarNoLeidasAsync(int usuarioId)
        {
            return await _context.Notificaciones
                .CountAsync(n => n.UsuarioId == usuarioId && !n.Leida);
        }
    }
}