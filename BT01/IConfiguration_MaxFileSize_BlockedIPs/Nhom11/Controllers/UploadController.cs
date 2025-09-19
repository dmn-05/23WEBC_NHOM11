using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Nhom11.Controllers {
  public class UploadController : Controller {
    private readonly IConfiguration _config;
    public UploadController(IConfiguration config) {
      _config = config;
    }
    public IActionResult Index() {
      ViewBag.MaxFileSizeMB = _config["UpdateSettings:MaxFileSizeMB"];
      ViewBag.BlockedIPs = _config.GetSection("UpdateSettings:BlockedIPs").Get<string[]>();
      return View();
    }
  }
}
