namespace Tuan06.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var user = context.Session.GetString("User");
            var role = context.Session.GetInt32("Role");

            // Lấy đường dẫn (ví dụ: /Admin/Dashboard/Index)
            var path = context.Request.Path.Value?.ToLower() ?? "";

            // Kiểm tra vào khu vực admin
            if (path.StartsWith("/admin"))
            {
                if (user == null || role != 2)
                {
                    context.Response.Redirect("/Login/Index");
                    return;
                }
            }

            await _next(context);
        }
    }
}
