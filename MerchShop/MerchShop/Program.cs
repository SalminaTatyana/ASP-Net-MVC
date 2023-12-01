using IvanovaShop;
using IvanovaShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using IvanovaShop.Data;
using IvanovaShop.Areas.Identity.Data;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
// �������� ������ ����������� �� ����� ������������
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
string connectionIdentity = builder.Configuration.GetConnectionString("IvanovaIdentityShopContextConnection");

builder.Services.AddDbContext<IvanovaIdentityShopContext>(options =>
    options.UseSqlServer(connectionIdentity));

builder.Services.AddDefaultIdentity<IvanovaIdentityShopUser>(opts => {
    
    opts.Password.RequiredLength = 5;   // минимальная длина
    opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
    opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
    opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
    opts.Password.RequireDigit = false; // требуются ли цифры
    opts.User.RequireUniqueEmail = true;
    opts.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNMйцукенгшщзхъфывапролджэячсмитьбю.@ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ'";
}).AddEntityFrameworkStores<IvanovaIdentityShopContext>();
// ��������� �������� ApplicationContext � �������� ������� � ����������
var services = builder.Services;
services.AddRazorPages();
services.AddDbContext<IvanovaShop.IvanovaShopContext>(options => options.UseSqlServer(connection));
services.AddControllersWithViews().AddRazorRuntimeCompilation().AddSessionStateTempDataProvider();
services.AddDistributedMemoryCache();
services.AddSession();
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

app.UseAuthentication();    // ����������� ��������������
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
