using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tuan06.Models;
using Tuan06.Services;
namespace Tuan06.Controllers {
  public class HomeController : Controller {
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productService;
    //Tiêm DI
    public HomeController(IProductService productService, ILogger<HomeController> logger) {
      _productService = productService;
      _logger = logger;
    }


    //Khai_Lấy 8 sp đầu tiên trong danh sách
    public IActionResult Index() {
      var products = _productService.GetAll().Take(8).ToList();
      return View(products); //truyen sang view
    }

    public IActionResult Contact() {
      return View();
    }
    public IActionResult Typo() {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
