using Microsoft.AspNetCore.Mvc;
using Tuan06.Data;
using Tuan06.Models;

namespace Web05_Nhom11.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ProductDAL _productDAL;

        public ProductController(ProductDAL productDAL)
        {
            _productDAL = productDAL;
        }

        public IActionResult Index()
        {
            var products = _productDAL.GetAllProducts();
            return View(products);
        }
        [HttpGet]
        public IActionResult Add() => View();

        [HttpGet]
        public IActionResult Create(Product product, IFormFile ProductImage)
        {
            if (ProductImage != null && ProductImage.Length > 0)
            {
                string fileName = Path.GetFileName(ProductImage.FileName);
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                string filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ProductImage.CopyTo(stream);
                }

                product.ProductImage = "images/products/" + fileName;
            }

            if (ModelState.IsValid)
            {
                _productDAL.AddProduct(product);
                return RedirectToAction("Index");
            }

            return View(product);
        }

    }
}
