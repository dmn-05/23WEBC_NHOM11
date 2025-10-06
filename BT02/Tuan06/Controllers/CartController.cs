//begin phat
using Microsoft.AspNetCore.Mvc;
using Tuan06.Models;
using Tuan06.Services;
using Tuan06.Helpers;

namespace Tuan06.Controllers {
  public class CartController : Controller {
    private const string CART_KEY = "CART";
    private readonly IProductService _productService;

    public CartController(IProductService productService)
        => _productService = productService;

    public IActionResult Index() {
      var cart = HttpContext.Session.GetObject<List<CartItem>>(CART_KEY) ?? new List<CartItem>();

      // ✅ Cập nhật lại Session để layout đọc được
      UpdateCartSession(cart);

      ViewBag.Total = cart.Sum(x => x.SubTotal);
      return View(cart);
    }

    [HttpGet]
    public IActionResult Add(int id) {
      var sp = _productService.GetById(id);
      if (sp == null) return NotFound();

      var cart = HttpContext.Session.GetObject<List<CartItem>>(CART_KEY) ?? new List<CartItem>();

      var item = cart.FirstOrDefault(x => x.MaSP == id);
      if (item == null) {
        cart.Add(new CartItem {
          MaSP = sp.MaSP,
          TenSP = sp.TenSP,
          HinhAnh = sp.HinhAnh,
          DonGia = sp.DonGia,
          DonGiaKhuyenMai = sp.DonGiaKhuyenMai,
          Qty = 1
        });
      } else item.Qty++;

      // ✅ Lưu lại giỏ hàng và cập nhật thông tin
      HttpContext.Session.SetObject(CART_KEY, cart);
      UpdateCartSession(cart);

      return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Remove(int id) {
      var cart = HttpContext.Session.GetObject<List<CartItem>>(CART_KEY) ?? new List<CartItem>();
      cart.RemoveAll(x => x.MaSP == id);

      HttpContext.Session.SetObject(CART_KEY, cart);
      UpdateCartSession(cart);

      return RedirectToAction("Index");
    }

    // 🔹 Hàm riêng để cập nhật số lượng và tổng tiền vào Session
    private void UpdateCartSession(List<CartItem> cart) {
      HttpContext.Session.SetInt32("CartCount", cart.Sum(x => x.Qty));
      HttpContext.Session.SetString("CartTotal", cart.Sum(x => x.SubTotal).ToString("N0"));
    }
  }
}
//end phat
