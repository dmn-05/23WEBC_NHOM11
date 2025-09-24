using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace _23WEBC_Nhom11_Tuan04.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _logFilePath = "request.log";

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var url = context.Request.Path;
            var method = context.Request.Method;
            var ip = context.Connection.RemoteIpAddress?.ToString();
            var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Tạo dòng log
            var logLine = $"[{time}] {method} {url} - IP: {ip}";

            // Ghi log vào file
            await File.AppendAllTextAsync(_logFilePath, logLine + Environment.NewLine);

            // Gọi middleware tiếp theo trong pipeline
            await _next(context);
        }

    }
}
