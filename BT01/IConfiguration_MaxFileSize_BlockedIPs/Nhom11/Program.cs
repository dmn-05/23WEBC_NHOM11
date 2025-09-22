using Nhom11;
using Nhom11.Services;

var builder = WebApplication.CreateBuilder(args);
//Dang ky UserService cho DI dang Scoped -Khai 21/09/2025
builder.Services.AddScoped<IUserService, UserService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<UpdateSettings>(
  builder.Configuration.GetSection("UploadSettings")
);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
