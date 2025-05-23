using Microsoft.AspNetCore.Mvc;

namespace Red_Social_Voluntarios.Controllers
{
    public class ReconocimientoController : Controller
    {
        public IActionResult Reconocimiento()
        {
            return View("Reconocimiento");
        }
    }
}
