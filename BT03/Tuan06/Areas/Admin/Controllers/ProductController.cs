
using Microsoft.AspNetCore.Mvc;
using Tuan06.Models;

using Tuan06.Data;

namespace Web05_Nhom11.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ProductDAL _productDAL;
        private readonly CategoryDAL _categoryDAL;
        public ProductController(IConfiguration configuration)
        {
            _productDAL = new ProductDAL(configuration);
            _categoryDAL = new CategoryDAL(configuration);
        }

      

        // GET: /Product/Create
        public IActionResult Create()
        {
            //Lấy danh sách danh mục từ CategoryDAL
            var categories = _categoryDAL.GetAllCategories();
            ViewBag.Categories = categories;

            return View();
        }

        // POST: /Product/Create
        [HttpPost]
        public IActionResult Create(Product product, IFormFile ProductImage)
        {
            if (ModelState.IsValid)
            {
                //Upload ảnh nếu có
                if (ProductImage != null && ProductImage.Length > 0)
                {
                    var fileName = Path.GetFileName(ProductImage.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/user/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ProductImage.CopyTo(stream);
                    }

                    product.ProductImage = "user/images/" + fileName;
                }

                _productDAL.AddProduct(product); // thêm sản phẩm vào DB
                Console.WriteLine("Đã gọi AddProduct");
                TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
                return RedirectToAction("Index");
            }

            // Nếu có lỗi, nạp lại danh mục để tránh null
            ViewBag.Categories = _categoryDAL.GetAllCategories();
            return View(product);
        }

        //POST: /Product/AddCategory
        [HttpPost]
        public JsonResult AddCategory([FromBody] dynamic data)
        {
            string categoryName = data?.categoryName;
            if (string.IsNullOrWhiteSpace(categoryName))
                return Json(new { success = false, message = "Tên danh mục trống" });

            _categoryDAL.AddCategory(categoryName);
            return Json(new { success = true });
        }
        public IActionResult Index()
        {
            var products = _productDAL.GetAllProducts();
            return View(products); 
        }

    }
}

