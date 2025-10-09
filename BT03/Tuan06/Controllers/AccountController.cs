using Microsoft.AspNetCore.Mvc;
using Tuan06.Data;

namespace Tuan06.Controllers
{
    public class AccountController : Controller
    {
            private readonly UserDAL _userDAL;

            public AccountController(UserDAL userDAL)
            {
                _userDAL = userDAL;
            }

            [HttpGet]
            public IActionResult Login()
            {
                return View();
            }

            [HttpPost]
            public IActionResult Login(string username, string password)
            {
                var user = _userDAL.Login(username, password);

                if (user != null)
                {
                    // Lưu thông tin vào session
                    HttpContext.Session.SetString("UserName", user.UserName ?? "");
                    HttpContext.Session.SetInt32("UserRole", user.UserRole);
                if (user.UserRole == 3)
                {
                    // Chuyển đến Area Admin
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                else if (user.UserRole == 1)
                {
                    // Chuyển đến trang user
                    return RedirectToAction("Index", "Home");
                }
            }

                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng!";
                return View();
            }

            public IActionResult Logout()
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login");
            }
        }
}
