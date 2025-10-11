using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tuan06.Data;
using Tuan06.Models;

namespace Tuan06.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReviewController : Controller
    {
        private readonly MyDbContext _context;
        private const int PageSize = 10;

        public ReviewController(MyDbContext context)
        {
            _context = context;
        }

        // ✅ Danh sách + tìm kiếm + phân trang
        public IActionResult Index(string? search, int page = 1)
        {
            var reviews = _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                reviews = reviews.Where(r =>
                    r.User.FullName.Contains(search) ||
                    r.Product.ProductName.Contains(search));
            }

            int total = reviews.Count();
            var data = reviews
                .OrderByDescending(r => r.CreatedAt)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)PageSize);

            return View(data);
        }

        // ✅ Chỉnh sửa nội dung review
        public IActionResult Edit(int id)
        {
            var review = _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Product)
                .FirstOrDefault(r => r.ReviewID == id);
            if (review == null) return NotFound();

            return View(review);
        }

        [HttpPost]
        public IActionResult Edit(Review review)
        {
            if (!ModelState.IsValid) return View(review);

            _context.Reviews.Update(review);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // ✅ Xóa review
        public IActionResult Delete(int id)
        {
            var review = _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Product)
                .FirstOrDefault(r => r.ReviewID == id);
            if (review == null) return NotFound();

            return View(review);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var review = _context.Reviews.Find(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
