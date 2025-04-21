using Microsoft.AspNetCore.Mvc;
using Red_Social_Voluntarios.Models;
using System.Collections.Generic;
using System.Linq;

namespace Red_Social_Voluntarios.Controllers
{
    public class UsuarioController : Controller
    {
        // Simulamos base de datos con una lista
        private static List<Usuario> usuarios = new();

        // Vista Registro (GET)
        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        // Procesa registro (POST)
        [HttpPost]
        public IActionResult Registro(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuarios.Add(usuario);
                ViewBag.Mensaje = "Registro exitoso.";
                return View();
            }

            return View(usuario);
        }

        // Vista Login (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Procesa Login (POST)
        [HttpPost]
        public IActionResult Login(string correo, string contrasena)
        {
            var usuario = usuarios.FirstOrDefault(u => u.Correo == correo && u.Contrasena == contrasena);

            if (usuario != null)
            {
                ViewBag.Mensaje = $"Bienvenido, {usuario.NombreUsuario}";
            }
            else
            {
                ViewBag.Mensaje = "Correo o contraseña incorrectos.";
            }

            return View();
        }
    }
}
