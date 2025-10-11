using Microsoft.AspNetCore.Mvc;
using Tuan06.Data;
using Tuan06.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Tuan06.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly MyDbContext _context;
        private const int PageSize = 10;

        public UserController(MyDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách + tìm kiếm + phân trang
        public IActionResult Index(string? search, int? role, int page = 1)
        {
            var users = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                users = users.Where(u => u.FullName.Contains(search) || u.UserName.Contains(search));

            if (role.HasValue)
                users = users.Where(u => u.UserRole == role.Value.ToString());

            int totalUsers = users.Count();
            var data = users
                .OrderBy(u => u.UserID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentSearch = search;
            ViewBag.CurrentRole = role;
            ViewBag.TotalPages = (int)Math.Ceiling(totalUsers / (double)PageSize);
            ViewBag.Page = page;

            return View(data);
        }

        // Thêm người dùng
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(User user)
        {
            if (_context.Users.Any(u => u.UserName == user.UserName))
                ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại");

            if (_context.Users.Any(u => u.Email == user.Email))
                ModelState.AddModelError("Email", "Email đã tồn tại");

            if (!ModelState.IsValid) return View(user);

            user.CreatedAt = DateTime.Now;
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // Sửa
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (!ModelState.IsValid) return View(user);

            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // Khóa / Mở khóa tài khoản
        public IActionResult ToggleStatus(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                user.UserStatus = !user.UserStatus;
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        // Xóa người dùng
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Include(u => u.Orders).FirstOrDefault(u => u.UserID == id);
            if (user == null) return NotFound();

            // Nếu user có đơn hàng → đổi sang trạng thái bị khóa
            if (_context.Orders.Any(o => o.UserID == id))
            {
                user.UserStatus = false; // bị khóa
                _context.SaveChanges();
            }
            else
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
