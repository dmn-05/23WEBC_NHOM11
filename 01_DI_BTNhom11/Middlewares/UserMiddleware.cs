using _01_DI_BTNhom11.Services;

namespace _01_DI_BTNhom11.Middlewares
{
    public class UserMiddleware
    {
        private readonly RequestDelegate _next;

        public UserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserService userService)
        {
            // Load từ file JSON (hoặc DB, hoặc hardcode)
            var usersJson = await File.ReadAllTextAsync("users.json");
            var users = System.Text.Json.JsonSerializer.Deserialize<List<User>>(usersJson);

            userService.SetUsers(users);

            // Chuyển tiếp request
            await _next(context);
        }
    }

}
