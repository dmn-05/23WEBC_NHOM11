using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tuan06.Data;
using Tuan06.Models;
namespace Tuan06.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductDAL _productDAL;
        public HomeController(IConfiguration configuration)
        {
            _productDAL = new ProductDAL(configuration);
        }
        //Khai_Lấy 8 sp đầu tiên trong danh sách
        public IActionResult Index()
        {
            var products= _productDAL.GetAllProducts();
            return View(products); //truyen sang view
        }

        public IActionResult Contact()
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
