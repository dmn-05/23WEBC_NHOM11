using Microsoft.Extensions.FileProviders;
using System.IO;
using Tuan06.Services;
var builder = WebApplication.CreateBuilder(args);
// Begin Nhat
builder.Services.AddSingleton<IProductService, ProductService>();
// End Nhat
// Add services to the container.
builder.Services.AddControllersWithViews();
// begin phat
builder.Services.AddSession();
// end phat
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// bigin phat
app.UseSession();

// end phat
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");




app.Run();
