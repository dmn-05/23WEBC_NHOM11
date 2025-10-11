using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Tuan06.Data;

var builder = WebApplication.CreateBuilder(args);

// Begin Khai
//builder.Services.AddSingleton<IProductService, ProductService>();
// End Khai
builder.Services.AddTransient<Tuan06.Data.ProductDAL>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Begin Phat
// Th�m c?u h?nh k?t n?i EF Core (DbContext)
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//Emd Phat

// ? Th�m DistributedMemoryCache (n?u kh�ng c� s? b�o l?i khi Session ch?y)
builder.Services.AddDistributedMemoryCache();

// begin phat
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // th?i gian s?ng c?a session
    options.Cookie.HttpOnly = true; // t�ng b?o m?t
    options.Cookie.IsEssential = true; // session ho?t �?ng k? c? khi user t?t consent cookie
});
// end phat
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// begin phat
app.UseSession(); // ph?i �?t tr�?c UseAuthorization v� MapControllerRoute
// end phat
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
