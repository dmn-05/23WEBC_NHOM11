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
        //public IActionResult Index()
        //{
        //    var products= _productDAL.GetAllProducts().Take(8).ToList();
        //    return View(products); //truyen sang view
        //}
        public IActionResult Index(int page = 1)
        {
            int pageSize = 8; // số sản phẩm mỗi trang

            var allProducts = _productDAL.GetAllProducts();
            int totalProducts = allProducts.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            var products = allProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(products);
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Typo()
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
