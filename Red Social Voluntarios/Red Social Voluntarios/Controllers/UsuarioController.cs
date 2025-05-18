using Microsoft.AspNetCore.Mvc;
using Red_Social_Voluntarios.Models;
using System.Collections.Generic;
using System.Linq;

namespace Red_Social_Voluntarios.Controllers
{
    public class UsuarioController : Controller
    {
        // Simulación de base de datos
        private static List<Usuario> usuarios = new();

        // Diccionario temporal para guardar códigos por correo
        private static Dictionary<string, string> codigosRecuperacion = new();

        private readonly EmailService _emailService;

        // Constructor para inyectar EmailService
        public UsuarioController(EmailService emailService)
        {
            _emailService = emailService;
        }

        // ====================
        // REGISTRO
        // ====================
        [HttpGet]
        public IActionResult Registro(string rol)
        {
            ViewBag.RolSeleccionado = rol;
            return View();
        }

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

        // ====================
        // LOGIN
        // ====================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

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

        // ====================
        // RECUPERAR CONTRASEÑA
        // ====================
        [HttpGet]
        public IActionResult Recuperar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Recuperar(string correo)
        {
            var usuario = usuarios.FirstOrDefault(u => u.Correo == correo);
            if (usuario == null)
            {
                ViewBag.Mensaje = "Correo no encontrado.";
                return View();
            }

            string codigo = new Random().Next(1000, 9999).ToString();
            codigosRecuperacion[correo] = codigo;

            _emailService.EnviarCodigo(correo, codigo);

            TempData["Correo"] = correo;
            return RedirectToAction("ValidarCodigo");
        }

        // ====================
        // VALIDAR CÓDIGO
        // ====================
        [HttpGet]
        public IActionResult ValidarCodigo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ValidarCodigo(string codigo)
        {
            string correo = TempData["Correo"] as string;

            if (correo == null || !codigosRecuperacion.ContainsKey(correo))
            {
                ViewBag.Mensaje = "Sesión expirada o inválida. Intenta nuevamente.";
                return RedirectToAction("Recuperar");
            }

            if (codigosRecuperacion[correo] == codigo)
            {
                TempData["Correo"] = correo;
                codigosRecuperacion.Remove(correo);
                return RedirectToAction("CambiarPassword");
            }
            else
            {
                ViewBag.Mensaje = "Código incorrecto.";
                TempData["Correo"] = correo; // Mantener el correo para intentar de nuevo
                return View();
            }
        }

        // ====================
        // CAMBIAR CONTRASEÑA
        // ====================
        [HttpGet]
        public IActionResult CambiarPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CambiarPassword(string nueva)
        {
            string correo = TempData["Correo"] as string;

            var usuario = usuarios.FirstOrDefault(u => u.Correo == correo);
            if (usuario != null)
            {
                usuario.Contrasena = nueva;
                TempData["Mensaje"] = "Contraseña actualizada correctamente. Inicia sesión con tu nueva contraseña.";
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Mensaje = "Ocurrió un error. Intenta de nuevo.";
                return View();
            }
        }

    }
}
