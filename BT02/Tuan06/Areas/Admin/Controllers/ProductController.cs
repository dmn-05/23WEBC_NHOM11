using Microsoft.AspNetCore.Mvc;
using Tuan06.Controllers;
using Tuan06.Models;
using Tuan06.Services;

namespace Web05_Nhom11.Areas.Admin.Controllers {
  [Area("Admin")]
  public class ProductController : Controller {
    private readonly ILogger<ProductController> _logger;
    private readonly IProductService _productService;
    public ProductController(IProductService productService, ILogger<ProductController> logger) {
      _productService = productService;
      _logger = logger;
    }
    public IActionResult Index() {
      var products = _productService.GetAll().ToList();
      return View(products);
    }
    [HttpGet]
    public IActionResult Add() => View();

    [HttpPost]
    public IActionResult Add(Product product) {
      if (ModelState.IsValid) {
        _productService.Add(product);
        return RedirectToAction("Index", "Product", new { area = "Admin" });
      }
      return View(product);
    }
    [HttpPost]
    public IActionResult Delete(int id) {
      _productService.Delete(id);
      return RedirectToAction("Index", "Product", new { area = "Admin" });
    }
  } 
}
