using Microsoft.AspNetCore.Mvc;

namespace Tuan06.Controllers {
  public class CartController : Controller {
      public IActionResult Index() {
        return View();
      }
  }
}
