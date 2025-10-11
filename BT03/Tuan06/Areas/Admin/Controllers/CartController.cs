using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tuan06.Data;
using Tuan06.Models;

namespace Tuan06.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CartController : Controller
    {
        private readonly MyDbContext _context;
        private const int PageSize = 10;

        public CartController(MyDbContext context)
        {
            _context = context;
        }

        // Danh sách giỏ hàng + tìm kiếm + phân trang
        public IActionResult Index(string? search, int page = 1)
        {
            var carts = _context.Carts
                .Include(c => c.User)
                .Include(c => c.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
                carts = carts.Where(c => c.User.FullName.Contains(search) || c.User.UserName.Contains(search));

            int total = carts.Count();
            var data = carts
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)PageSize);

            return View(data);
        }

        // Sửa số lượng trong giỏ
        public IActionResult Edit(int userId, int productId)
        {
            var cart = _context.Carts
                .Include(c => c.User)
                .Include(c => c.Product)
                .FirstOrDefault(c => c.UserID == userId && c.ProductID == productId);

            if (cart == null) return NotFound();
            return View(cart);
        }

        [HttpPost]
        public IActionResult Edit([Bind("UserID,ProductID,Quantity")] Cart cart)
        {
            var old = _context.Carts.FirstOrDefault(c => c.UserID == cart.UserID && c.ProductID == cart.ProductID);
            if (old == null) return NotFound();

            old.Quantity = cart.Quantity;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // Xóa giỏ hàng
        public IActionResult Delete(int userId, int productId)
        {
            var cart = _context.Carts
                .Include(c => c.User)
                .Include(c => c.Product)
                .FirstOrDefault(c => c.UserID == userId && c.ProductID == productId);

            if (cart == null) return NotFound();
            return View(cart);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int userId, int productId)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.UserID == userId && c.ProductID == productId);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        // Xóa giỏ của user bị khóa
        public IActionResult DeleteInactiveUsers()
        {
            var inactiveUserIds = _context.Users
                .Where(u => u.UserStatus == false)
                .Select(u => u.UserID)
                .ToList();

            var carts = _context.Carts
                .Where(c => inactiveUserIds.Contains(c.UserID))
                .ToList();

            _context.Carts.RemoveRange(carts);
            _context.SaveChanges();

            TempData["Message"] = $"Đã xóa {carts.Count} giỏ hàng của tài khoản bị khóa.";
            return RedirectToAction(nameof(Index));
        }
    }
}
