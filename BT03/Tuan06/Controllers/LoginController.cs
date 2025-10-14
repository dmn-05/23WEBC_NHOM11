using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Tuan06.Controllers
{
	public class LoginController : Controller
	{
		private readonly IConfiguration _config;

		public LoginController(IConfiguration config)
		{
			_config = config;
		}
		
		[HttpGet] //hien thi view index login
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Index(string username, string password)
		{
            //Lay chuoi ket noi den csdl (Connection String) tu file appsettings.json
            string _connectionString = _config.GetConnectionString("DefaultConnection");
          
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM [USER] WHERE USERName = @Username AND USERPassword = @Password";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Lay thong tin
                            int role = Convert.ToInt32(reader["UserRole"]);
                            string name = reader["Username"].ToString();

                            // Luu Session
                            HttpContext.Session.SetString("User", name);
                            HttpContext.Session.SetInt32("Role", role);
                            if (role == 2) // kiem tra neu la admin
                            {
                                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu!";
                        }
                    }
                }
            }

            return View();
		}
		
		public IActionResult Logout()
		{
			HttpContext.Session.Remove("User");//Xoa session
			return RedirectToAction("Index", "Home", new { area = "" }); // RedirectToAction(action,controller,area)
        }
	}
}
