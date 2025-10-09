using Microsoft.AspNetCore.Mvc;
using Tuan06.Data;
using Tuan06.Models;

namespace Tuan06.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductDAL _productDAL;

        public ProductController(ProductDAL productDAL)
        {
            _productDAL = productDAL;
        }

        // Hiển thị danh sách sản phẩm
        public IActionResult Index()
        {
            var products = _productDAL.GetAllProducts();
            return View(products);
        }

        // Hiển thị chi tiết sản phẩm
        [Route("Product/Detail/{id}")]
        public IActionResult Detail(int id)
        {
            var product = _productDAL.GetProductById(id);
            if (product == null)
                return NotFound();

            return View(product);
        }
        public IActionResult Search()
        {
            return View();
        }
    }
}
