using Microsoft.AspNetCore.Mvc;

namespace WebsiteDesafio2.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");
            var correoElectronico = HttpContext.Session.GetString("CorreoElectronico");
            var rolUsuario = HttpContext.Session.GetString("RolUsuario");
            var nombre = HttpContext.Session.GetString("Nombre");
            var apellidos = HttpContext.Session.GetString("Apellidos");

            if (!string.IsNullOrEmpty(nombreUsuario))
            {
                
            }
            else
            {
                TempData["Error"] = "No se encontraron datos en la sesión.";
                return RedirectToAction("Index", "Auth");
            }

            return View("Home");
        }
    }
}
