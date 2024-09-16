using Microsoft.AspNetCore.Mvc;

namespace WebsiteDesafio2.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View("Home");
        }
    }
}
