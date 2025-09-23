using Microsoft.AspNetCore.Mvc;

namespace _01_DI_BTNhom11.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index(int page = 1, int pageSize = 5)
        {
            var users = _userService.GetUsers();
            var totalUsers = users.Count;
            var totalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);

            // Lấy user theo trang
            var pagedUsers = users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new UserListViewModel
            {
                Users = pagedUsers,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(model);
        }
    }
}
