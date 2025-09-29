using Microsoft.AspNetCore.Mvc;

namespace Tuan06.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Single()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
