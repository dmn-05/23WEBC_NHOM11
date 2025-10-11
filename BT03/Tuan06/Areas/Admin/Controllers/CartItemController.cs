using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tuan06.Data;
using Tuan06.Models;

namespace Tuan06.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CartItemController : Controller
    {
        private readonly MyDbContext _context;
        private const int PageSize = 10;

        public CartItemController(MyDbContext context)
        {
            _context = context;
        }

        //  Danh sách chi tiết giỏ hàng
        public IActionResult Index(int? userId, string? search, int page = 1)
        {
            var carts = _context.Carts
                .Include(c => c.User)
                .Include(c => c.Product)
                .AsQueryable();

            // Nếu có userId => chỉ xem giỏ của user đó
            if (userId.HasValue)
                carts = carts.Where(c => c.UserID == userId);

            // Tìm theo tên sản phẩm
            if (!string.IsNullOrEmpty(search))
                carts = carts.Where(c => c.Product.ProductName.Contains(search));

            int total = carts.Count();
            var data = carts
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)PageSize);
            ViewBag.UserId = userId;

            return View(data);
        }

        //  Sửa số lượng sản phẩm trong giỏ
        public IActionResult Edit(int userId, int productId)
        {
            var item = _context.Carts
                .Include(c => c.User)
                .Include(c => c.Product)
                .FirstOrDefault(c => c.UserID == userId && c.ProductID == productId);

            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit([Bind("UserID,ProductID,Quantity")] Cart item)
        {
            var old = _context.Carts.FirstOrDefault(c => c.UserID == item.UserID && c.ProductID == item.ProductID);
            if (old == null) return NotFound();

            old.Quantity = item.Quantity;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        //  Xóa item khỏi giỏ
        public IActionResult Delete(int userId, int productId)
        {
            var item = _context.Carts
                .Include(c => c.User)
                .Include(c => c.Product)
                .FirstOrDefault(c => c.UserID == userId && c.ProductID == productId);

            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int userId, int productId)
        {
            var item = _context.Carts.FirstOrDefault(c => c.UserID == userId && c.ProductID == productId);
            if (item != null)
            {
                _context.Carts.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        //  Xóa tất cả item của user bị khóa
        public IActionResult DeleteInactiveUsers()
        {
            var inactiveUserIds = _context.Users
                .Where(u => u.UserStatus == false)
                .Select(u => u.UserID)
                .ToList();

            var items = _context.Carts
                .Where(c => inactiveUserIds.Contains(c.UserID))
                .ToList();

            _context.Carts.RemoveRange(items);
            _context.SaveChanges();

            TempData["Message"] = $"Đã xóa {items.Count} sản phẩm của user bị khóa.";
            return RedirectToAction(nameof(Index));
        }
    }
}
