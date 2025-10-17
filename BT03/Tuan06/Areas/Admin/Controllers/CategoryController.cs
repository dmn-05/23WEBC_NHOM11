
using Microsoft.AspNetCore.Mvc;

using Tuan06.Data;

namespace Web05_Nhom11.Areas.Admin.Controllers {
  [Area("Admin")]
  [Route("Admin/[controller]/[action]")]
  public class CategoryController : Controller {
    private readonly CategoryDAL _categoryDAL;
    public CategoryController(IConfiguration configuration) {
      _categoryDAL = new CategoryDAL(configuration);
    }

    // POST: /Category/AddCategory
    // API thêm danh mục (AJAX)
    [HttpPost]
    public JsonResult AddCategory([FromBody] CategoryData data) {
      if (string.IsNullOrWhiteSpace(data.CategoryName))
        return Json(new { success = false, message = "Tên danh mục trống!" });

      // Lưu vào database
      int newId = _categoryDAL.AddCategory(data.CategoryName);

      // Trả JSON về client
      return Json(new { success = true, id = newId, name = data.CategoryName });
    }

    public class CategoryData {
      public string CategoryName { get; set; }
    }
  }
}

