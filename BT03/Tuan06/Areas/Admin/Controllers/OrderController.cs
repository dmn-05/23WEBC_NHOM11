using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tuan06.Data;
using Tuan06.Models;

namespace Tuan06.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly MyDbContext _context;
        private const int PageSize = 10;

        public OrderController(MyDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách đơn hàng
        public IActionResult Index(string? search, string? status, DateTime? date, int page = 1)
        {
            var orders = _context.Orders.Include(o => o.User).AsQueryable();

            if (!string.IsNullOrEmpty(search))
                orders = orders.Where(o => o.User.FullName.Contains(search));

            if (!string.IsNullOrEmpty(status))
                orders = orders.Where(o => o.OrderStatus == status);

            if (date.HasValue)
                orders = orders.Where(o => o.CreatedAt.Date == date.Value.Date);

            int totalOrders = orders.Count();
            var data = orders
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentSearch = search;
            ViewBag.CurrentStatus = status;
            ViewBag.CurrentDate = date?.ToString("yyyy-MM-dd");
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalOrders / (double)PageSize);

            return View(data);
        }

        // Xem chi tiết đơn hàng
        public IActionResult Details(int id)
        {
            var order = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderID == id);

            if (order == null) return NotFound();
            return View(order);
        }

        // Sửa trạng thái
        public IActionResult Edit(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderID == id);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost]
        public IActionResult Edit(Order order)
        {
            if (!ModelState.IsValid) return View(order);

            var old = _context.Orders.FirstOrDefault(o => o.OrderID == order.OrderID);
            if (old == null) return NotFound();

            // chỉ cho phép cập nhật trạng thái nếu chưa hoàn tất
            if (old.OrderStatus != "Hoàn tất")
            {
                old.OrderStatus = order.OrderStatus;
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // Xóa đơn hàng
        public IActionResult Delete(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.OrderID == id);

            if (order == null) return NotFound();

            // chỉ cho xóa nếu chưa hoàn tất
            if (order.OrderStatus == "Chờ xử lý" || order.OrderStatus == "Đang giao")
            {
                // Cập nhật lại tồn kho sản phẩm
                foreach (var item in order.OrderDetails)
                {
                    var product = _context.Products.FirstOrDefault(p => p.ProductID == item.ProductID);
                    if (product != null)
                        product.Quantity += item.Quantity;
                }

                _context.Orders.Remove(order);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
