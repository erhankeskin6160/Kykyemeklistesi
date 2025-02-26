using Kykyemeklistesi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System;

using System.Globalization;

var cultureInfo = new CultureInfo("tr-TR");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

builder.Services.AddAuthentication("Cookies") 
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Admin/Login";          
        options.AccessDeniedPath = "/Admin/AccessDenied";
    });
var app = builder.Build();

 app.UseStatusCodePagesWithRedirects("/404Error.html");
 

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

app.UseAuthentication();
app.UseAuthorization();

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
    name: "cityFoodMenu",
    pattern: "{selectedCity}-bugunyemeklistesi",
    defaults: new { controller = "Home", action = "Bugunyemeklistesi" });

app.MapControllerRoute(
    name: "cityFoodMenuIndex",
    pattern: "{selectedCity}-yemeklistesi", // farklý pattern, örn. "-yemeklistesi" ekledik
    defaults: new { controller = "Home", action = "Index" });


app.MapControllerRoute(
     name: "default",
     pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
