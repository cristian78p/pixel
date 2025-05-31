using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Red_Social_Voluntarios.Data;
using Red_Social_Voluntarios.Models;
using Red_Social_Voluntarios.Services;
using System.Security.Claims;

namespace Red_Social_Voluntarios.Controllers
{
    [Authorize]
    public class EventoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly NotificacionService _notificacionService;
        private readonly EmailService _emailService;

        public EventoController(ApplicationDbContext context, NotificacionService notificacionService, EmailService emailService)
        {
            _context = context;
            _notificacionService = notificacionService;
            _emailService = emailService;
        }

        // GET: Evento
        public async Task<IActionResult> Index()
        {
            var eventos = await _context.Eventos
                .Include(e => e.Organizador)
                .Where(e => e.Activo)
                .OrderByDescending(e => e.FechaCreacion)
                .ToListAsync();
            return View(eventos);
        }

        // GET: Evento/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var evento = await _context.Eventos
                .Include(e => e.Organizador)
                .Include(e => e.Inscripciones)
                    .ThenInclude(i => i.Voluntario)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (evento == null) return NotFound();

            ViewBag.YaInscrito = false;
            if (User.Identity.IsAuthenticated)
            {
                var userId = GetCurrentUserId();
                ViewBag.YaInscrito = evento.Inscripciones.Any(i => i.VoluntarioId == userId);
            }

            return View(evento);
        }

        // GET: Evento/Create
        [Authorize(Policy = "SoloOrganizaciones")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Evento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "SoloOrganizaciones")]
        public async Task<IActionResult> Create(Evento evento)
        {
            if (ModelState.IsValid)
            {
                evento.OrganizadorId = GetCurrentUserId();
                _context.Add(evento);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Evento creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(evento);
        }

        // GET: Evento/Edit/5
        [Authorize(Policy = "SoloOrganizaciones")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null) return NotFound();

            if (evento.OrganizadorId != GetCurrentUserId())
                return Forbid();

            return View(evento);
        }

        // POST: Evento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "SoloOrganizaciones")]
        public async Task<IActionResult> Edit(int id, Evento evento)
        {
            if (id != evento.Id) return NotFound();

            var eventoExistente = await _context.Eventos.FindAsync(id);
            if (eventoExistente == null) return NotFound();

            if (eventoExistente.OrganizadorId != GetCurrentUserId())
                return Forbid();

            if (ModelState.IsValid)
            {
                eventoExistente.Titulo = evento.Titulo;
                eventoExistente.Descripcion = evento.Descripcion;
                eventoExistente.FechaInicio = evento.FechaInicio;
                eventoExistente.FechaFin = evento.FechaFin;
                eventoExistente.Ubicacion = evento.Ubicacion;
                eventoExistente.Ciudad = evento.Ciudad;
                eventoExistente.VoluntariosNecesarios = evento.VoluntariosNecesarios;
                eventoExistente.Requisitos = evento.Requisitos;
                eventoExistente.HabilidadesNecesarias = evento.HabilidadesNecesarias;
                eventoExistente.Categoria = evento.Categoria;

                _context.Update(eventoExistente);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Evento actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(evento);
        }

        // POST: Evento/Inscribirse/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "SoloVoluntarios")]
        public async Task<IActionResult> Inscribirse(int id, string? comentarios)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null) return NotFound();

            var userId = GetCurrentUserId();
            var yaInscrito = await _context.InscripcionesEventos
                .AnyAsync(i => i.EventoId == id && i.VoluntarioId == userId);

            if (yaInscrito)
            {
                TempData["ErrorMessage"] = "Ya estás inscrito en este evento.";
                return RedirectToAction("Details", new { id });
            }

            if (evento.EstaCompleto)
            {
                TempData["ErrorMessage"] = "Este evento ya no tiene cupos disponibles.";
                return RedirectToAction("Details", new { id });
            }

            var inscripcion = new InscripcionEvento
            {
                EventoId = id,
                VoluntarioId = userId,
                Comentarios = comentarios
            };

            _context.InscripcionesEventos.Add(inscripcion);
            evento.VoluntariosInscritos++;
            _context.Update(evento);
            await _context.SaveChangesAsync();

            // Notificar al organizador
            await _notificacionService.CrearNotificacionAsync(
                evento.OrganizadorId,
                "Nueva inscripción",
                $"Un voluntario se ha inscrito a tu evento: {evento.Titulo}",
                "Success");

            TempData["SuccessMessage"] = "Te has inscrito exitosamente al evento.";
            return RedirectToAction("Details", new { id });
        }

        // POST: Evento/CancelarInscripcion/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "SoloVoluntarios")]
        public async Task<IActionResult> CancelarInscripcion(int id)
        {
            var userId = GetCurrentUserId();
            var inscripcion = await _context.InscripcionesEventos
                .Include(i => i.Evento)
                .FirstOrDefaultAsync(i => i.EventoId == id && i.VoluntarioId == userId);

            if (inscripcion == null) return NotFound();

            _context.InscripcionesEventos.Remove(inscripcion);
            inscripcion.Evento.VoluntariosInscritos--;
            _context.Update(inscripcion.Evento);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Has cancelado tu inscripción al evento.";
            return RedirectToAction("Details", new { id });
        }

        // GET: Evento/MisEventos
        public async Task<IActionResult> MisEventos()
        {
            var userId = GetCurrentUserId();
            var usuario = await _context.Usuarios.FindAsync(userId);

            if (usuario.EsOrganizacion())
            {
                var eventosCreados = await _context.Eventos
                    .Where(e => e.OrganizadorId == userId)
                    .OrderByDescending(e => e.FechaCreacion)
                    .ToListAsync();
                return View("MisEventosOrganizacion", eventosCreados);
            }
            else
            {
                var inscripciones = await _context.InscripcionesEventos
                    .Include(i => i.Evento)
                        .ThenInclude(e => e.Organizador)
                    .Where(i => i.VoluntarioId == userId)
                    .OrderByDescending(i => i.FechaInscripcion)
                    .ToListAsync();
                return View("MisInscripciones", inscripciones);
            }
        }

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(idClaim);
        }
    }
}