using Microsoft.AspNetCore.Mvc;
using Tuan06.Services;
using Tuan06.Models;

namespace Tuan06.Controllers {
  [Route("Product")]
  public class ProductController : Controller {
    private readonly IProductService _ProductService;
    // Begin Nhat
    public ProductController(IProductService ProductService) {
      _ProductService = ProductService;
    }

    [HttpGet("")]
    public IActionResult Index() {
      var products = _ProductService.GetAll(); // giả sử trả về List<Product>
      return View(products); // truyền qua View
    }

    // Begin Phat
    [HttpGet("detail/{id}")]
    public IActionResult Detail(int id)
    {
        var sp = _ProductService.GetById(id);
        if (sp == null) return NotFound();
        return View("Single", sp);
    }
    // End Phat
  }
}
