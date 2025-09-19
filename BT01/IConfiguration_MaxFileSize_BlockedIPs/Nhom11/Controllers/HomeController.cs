using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nhom11.Models;

namespace Nhom11.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;


        public HomeController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            string connectString = _config.GetConnectionString("DefaultConnection");
            string appname = _config["AppSettings:AppName"];
            string version = _config["AppSettings:Version"];
            ViewBag.AppName = appname;
            ViewBag.Version = version;
            ViewBag.ConnectString = connectString;
            ViewBag.Message = $"AppName: {appname}, Version: {version}, ConnectionDB: {connectString}";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
