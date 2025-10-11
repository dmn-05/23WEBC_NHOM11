using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using Tuan06.Data;
using Tuan06.Models;

namespace Tuan06.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ProductDAL _productDAL;
        private readonly IWebHostEnvironment _env;

        public ProductController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _productDAL = new ProductDAL(configuration);
            _env = env;
        }

        // Hiển thị danh sách sản phẩm với phân trang: ?page=1
        public IActionResult Index(int page = 1, int pageSize = 12)
        {
            var allProducts = _productDAL.GetAllProducts();
            var total = allProducts.Count;
            var products = allProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)System.Math.Ceiling(total / (double)pageSize);

            return View(products);
        }

        // GET: Admin/Product/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _productDAL.GetAllCategories();
            return View();
        }

        // POST: Admin/Product/Create
        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("ProductImage", "Vui lòng chọn tệp ảnh.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _productDAL.GetAllCategories();
                return View(product);
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "user", "images");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Lưu vào DB: chỉ tên file
                product.ProductImage = uniqueFileName;
                // OR lưu đường dẫn tương đối từ webroot
                // product.ProductImage = "user/images/" + uniqueFileName;
                _productDAL.AddProduct(product);
            }
            return RedirectToAction("Index");
        }
    }
}