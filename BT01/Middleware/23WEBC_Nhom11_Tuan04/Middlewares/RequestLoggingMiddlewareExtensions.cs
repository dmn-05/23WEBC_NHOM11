using Microsoft.AspNetCore.Builder;
namespace _23WEBC_Nhom11_Tuan04.Middlewares
{
    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
