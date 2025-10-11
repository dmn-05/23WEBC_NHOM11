using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tuan06.Data;
using Tuan06.Models;

namespace Tuan06.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderDetailController : Controller
    {
        private readonly MyDbContext _context;

        public OrderDetailController(MyDbContext context)
        {
            _context = context;
        }

        // Hiển thị chi tiết theo OrderID
        public IActionResult Index(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderID == orderId);

            if (order == null) return NotFound();
            return View(order);
        }

        // GET: sửa 1 dòng chi tiết → cần cả orderId + productId
        public IActionResult Edit(int orderId, int productId)
        {
            var detail = _context.OrderDetails
                .Include(od => od.Order)
                .Include(od => od.Product)
                .FirstOrDefault(od => od.OrderID == orderId && od.ProductID == productId);

            if (detail == null) return NotFound();
            if (detail.Order?.OrderStatus == "Hoàn tất")
            {
                TempData["Error"] = "Đơn hàng đã hoàn tất, không thể chỉnh sửa.";
                return RedirectToAction("Index", new { orderId });
            }
            return View(detail);
        }

        // POST: sửa (bind theo composite key)
        [HttpPost]
        public IActionResult Edit([Bind("OrderID,ProductID,Quantity,Price")] OrderDetail detail)
        {
            if (!ModelState.IsValid) return View(detail);

            var old = _context.OrderDetails
                .FirstOrDefault(od => od.OrderID == detail.OrderID && od.ProductID == detail.ProductID);
            if (old == null) return NotFound();

            var order = _context.Orders.FirstOrDefault(o => o.OrderID == old.OrderID);
            if (order?.OrderStatus == "Hoàn tất")
            {
                TempData["Error"] = "Đơn hàng đã hoàn tất, không thể chỉnh sửa.";
                return RedirectToAction("Index", new { orderId = old.OrderID });
            }

            // cập nhật số lượng/giá
            old.Quantity = detail.Quantity;
            old.Price = detail.Price;

            _context.SaveChanges();
            return RedirectToAction("Index", new { orderId = old.OrderID });
        }

        // In hóa đơn (không đổi)
        public IActionResult Print(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderID == orderId);

            if (order == null) return NotFound();
            return View(order);
        }
    }
}
