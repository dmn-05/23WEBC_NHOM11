using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tuan06.Data;
using Tuan06.Models;

namespace Tuan06.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly MyDbContext _context;
        private const int PageSize = 10;

        public TagController(MyDbContext context)
        {
            _context = context;
        }

        // ✅ Danh sách + tìm kiếm + phân trang
        public IActionResult Index(string? search, int page = 1)
        {
            var tags = _context.Tags.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                tags = tags.Where(t => t.TagName.Contains(search));

            int total = tags.Count();
            var data = tags
                .OrderByDescending(t => t.TagID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)PageSize);

            return View(data);
        }

        // ✅ Thêm tag
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Tag tag)
        {
            if (_context.Tags.Any(t => t.TagName == tag.TagName))
                ModelState.AddModelError("TagName", "Tên tag đã tồn tại");

            if (!ModelState.IsValid) return View(tag);

            tag.TagStatus = true;
            _context.Tags.Add(tag);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // ✅ Sửa tag
        public IActionResult Edit(int id)
        {
            var tag = _context.Tags.Find(id);
            if (tag == null) return NotFound();
            return View(tag);
        }

        [HttpPost]
        public IActionResult Edit(Tag tag)
        {
            if (!ModelState.IsValid) return View(tag);

            _context.Tags.Update(tag);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // ✅ Xóa tag (chỉ khi chưa liên kết sản phẩm)
        public IActionResult Delete(int id)
        {
            var tag = _context.Tags
                .Include(t => t.ProductTags)
                .FirstOrDefault(t => t.TagID == id);
            if (tag == null) return NotFound();
            return View(tag);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var tag = _context.Tags
                .Include(t => t.ProductTags)
                .FirstOrDefault(t => t.TagID == id);

            if (tag == null) return NotFound();

            if (tag.ProductTags != null && tag.ProductTags.Any())
            {
                TempData["Error"] = "Không thể xóa tag đang gắn với sản phẩm!";
            }
            else
            {
                _context.Tags.Remove(tag);
                _context.SaveChanges();
                TempData["Message"] = "Xóa thành công!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
