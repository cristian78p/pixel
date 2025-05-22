using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Red_Social_Voluntarios.Models;

namespace Red_Social_Voluntarios.Controllers
{
    public class ComunidadController : Controller
    {
        //Simulación de datos en memoria (sin base de datos)
        private static List<Comunidad> comunidades = new List<Comunidad>();

        // Método para mostrar todas las publicaciones en la comunidad
        public IActionResult Comunidad()
        {
            return View("Comunidad",comunidades);
        }

        //Método para mostrar el formulario de creación de una nueva publicación
        public IActionResult Crear()
        {
            return View();
        }

        //Método para procesar la creación de una nueva publicación
        [HttpPost]
        public IActionResult Crear(Comunidad comunidad)
        {
            comunidad.Id = comunidades.Count + 1; // Asigna un ID único simulado
            comunidad.FechaPublicacion = System.DateTime.Now; // Establece la fecha actual
            comunidades.Add(comunidad); // Agrega la nueva publicación a la lista en memoria
            return RedirectToAction("Comunidad"); // Redirige a la lista de publicaciones
        }

        //Método para ver los detalles de una publicación específica
        public IActionResult Detalle(int id)
        {
            var comunidad = comunidades.FirstOrDefault(c => c.Id == id);
            if (comunidad == null)
            {
                return NotFound(); // Manejo si la publicación no existe
            }
            return View(comunidad);
        }
    }
}
