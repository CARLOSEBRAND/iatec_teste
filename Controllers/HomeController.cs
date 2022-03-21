using Microsoft.AspNetCore.Mvc;

namespace EmprestimoBancario.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
