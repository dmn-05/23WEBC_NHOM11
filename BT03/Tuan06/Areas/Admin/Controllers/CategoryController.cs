using Microsoft.AspNetCore.Mvc;
using Tuan06.Data;

namespace Tuan06.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly CategoryDAL _categoryDAL;
        public CategoryController(CategoryDAL categoryDAL)
        {
            _categoryDAL = categoryDAL;
        }   
        public IActionResult Index()
        {
            var categories = _categoryDAL.GetAllCategories();
            return View(categories);
        }
    }
}
