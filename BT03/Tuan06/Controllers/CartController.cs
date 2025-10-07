using Microsoft.AspNetCore.Mvc;
using Tuan06.Models;
using Tuan06.Helpers;

namespace Tuan06.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}