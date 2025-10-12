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

        //Khai - Hien thi danh sach sp (12 sp _ co phan trang)
        public IActionResult Index(int page = 1)
        {
            int pageSize = 12; // Moi trang co 12 san pham
            int totalProducts;

            var products = _productDAL.GetProductsPaged(page, pageSize, out totalProducts);
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

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
       
    }
}
