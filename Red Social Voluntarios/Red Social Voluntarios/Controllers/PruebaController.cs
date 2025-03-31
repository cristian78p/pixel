using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Red_Social_Voluntarios.Data;

namespace Red_Social_Voluntarios.Controllers
{
    [Route("mi-prueba")]
    public class PruebaController : Controller
    {
        private readonly ContextoDB contextoDB;
        public PruebaController(ContextoDB contextoDB)
        {
            this.contextoDB = contextoDB;
        }
        [Route("test-db")]
        public IActionResult ProbarConexion()
        {
            try
            {
                contextoDB.Database.OpenConnection();
                contextoDB.Database.CloseConnection();
                return Content("Conexión exitosa a SQL Server.");
            }
            catch (Exception ex)
            {
                return Content("Error de conexión: " + ex.Message);
            }
        }
    }
}
