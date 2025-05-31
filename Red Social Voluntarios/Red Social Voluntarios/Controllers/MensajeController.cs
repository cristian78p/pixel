using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Red_Social_Voluntarios.Data;
using Red_Social_Voluntarios.Models;
using System.Security.Claims;

namespace Red_Social_Voluntarios.Controllers
{
    [Authorize]
    public class MensajeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MensajeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Mensaje
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var conversaciones = await _context.Mensajes
                .Include(m => m.Emisor)
                .Include(m => m.Receptor)
                .Where(m => m.EmisorId == userId || m.ReceptorId == userId)
                .GroupBy(m => m.EmisorId == userId ? m.ReceptorId : m.EmisorId)
                .Select(g => g.OrderByDescending(m => m.FechaEnvio).First())
                .ToListAsync();

            return View(conversaciones);
        }

        // GET: Mensaje/Conversacion/{id}
        public async Task<IActionResult> Conversacion(string id)
        {
            var userId = GetCurrentUserId();
            var otroUsuario = await _context.Usuarios.FindAsync(id);
            if (otroUsuario == null) return NotFound();

            var mensajes = await _context.Mensajes
                .Include(m => m.Emisor)
                .Include(m => m.Receptor)
                .Where(m => (m.EmisorId == userId && m.ReceptorId == id) ||
                            (m.EmisorId == id && m.ReceptorId == userId))
                .OrderBy(m => m.FechaEnvio)
                .ToListAsync();

            // Marcar mensajes como leídos
            var mensajesNoLeidos = mensajes.Where(m => m.ReceptorId == userId && !m.Leido);
            foreach (var mensaje in mensajesNoLeidos)
            {
                mensaje.Leido = true;
            }

            await _context.SaveChangesAsync();

            ViewBag.OtroUsuario = otroUsuario;
            return View(mensajes);
        }

        // POST: Mensaje/Enviar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enviar(string receptorId, string contenido)
        {
            if (string.IsNullOrWhiteSpace(contenido))
            {
                TempData["ErrorMessage"] = "El mensaje no puede estar vacío.";
                return RedirectToAction("Conversacion", new { id = receptorId });
            }

            var mensaje = new Mensaje
            {
                EmisorId = GetCurrentUserId(),
                ReceptorId = receptorId,
                Contenido = contenido.Trim(),
                FechaEnvio = DateTime.Now,
                Leido = false
            };

            _context.Mensajes.Add(mensaje);
            await _context.SaveChangesAsync();

            return RedirectToAction("Conversacion", new { id = receptorId });
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
