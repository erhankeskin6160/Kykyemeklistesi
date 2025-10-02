using Kykyemeklistesi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;

var cultureInfo = new CultureInfo("tr-TR");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Admin/Login";
        options.AccessDeniedPath = "/Admin/AccessDenied";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});



builder.Services.AddControllersWithViews(options =>
{
 });

var app = builder.Build();

app.UseStatusCodePagesWithRedirects("/404Error.html");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Özel route'larý önce tanýmlýyoruz
app.MapControllerRoute(
    name: "Login",
    pattern: "Login",
    defaults: new { controller = "User", action = "Login" }
);

app.MapControllerRoute(
    name: "Register",
    pattern: "Register",
    defaults: new { controller = "User", action = "Register" }
);

app.MapControllerRoute(
    name: "404Error",
    pattern: "404Error.html",
    defaults: new { controller = "Home", action = "NotFound" }
);

app.MapControllerRoute(
    name: "sitemap",
    pattern: "sitemap.xml",
    defaults: new { controller = "Home", action = "GenerateSitemap" });

app.MapControllerRoute(
   name: "cityTodayMenu",
   pattern: "{selectedCity}-bugun-yemek-listesi",
   defaults: new { controller = "Home", action = "Bugunyemeklistesi" });

// Þehir bazlý genel yemek listesi için SEO dostu URL  
app.MapControllerRoute(
    name: "cityMenu",
    pattern: "{selectedCity}-yemek-listesi",
    defaults: new { controller = "Home", action = "Index" });

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();