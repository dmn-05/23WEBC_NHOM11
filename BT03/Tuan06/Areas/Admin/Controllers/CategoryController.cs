using Microsoft.AspNetCore.Mvc;
using Tuan06.Data;
using Tuan06.Models;

namespace Tuan06.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ProductDAL _productDAL;

        public CategoryController(IConfiguration configuration)
        {
            _productDAL = new ProductDAL(configuration);
        }

        // Hiển thị danh sách danh mục     
        public IActionResult Index(int page = 1, int pageSize = 5)
        {
            var allProducts = _productDAL.GetAllCategories();
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
        // GET: Admin/Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _productDAL.AddCategory(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

    }
}