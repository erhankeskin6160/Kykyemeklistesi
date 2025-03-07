﻿using Kykyemeklistesi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace Kykyemeklistesi.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {


        private static AppDbContext _db;

        public AdminController(AppDbContext appContext)
        {
            _db = appContext;
        }


        public IActionResult Index()
        {
           
            return View();
        }
        public IActionResult YemekListesiEkle()
        {
            var sehir = _db.Cities.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.CityName
            }).ToList();
            ViewBag.Sehir = sehir;
            return View();
        }


        [HttpPost]
        public IActionResult YemekListesiEkle(Yemek yemek)
        {


            _db.YemekListesi.Add(yemek);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult YemekListesi()
        {

            var yemekListesi = _db.YemekListesi.Include(x => x.City).OrderBy(x => x.Day).Where(x => x.Day.Month == DateTime.Now.Month &&
                x.Day.Year == DateTime.Now.Year).ToList();
            var sehir = _db.Cities.Select(x => new SelectListItem { Value = x.CityName, Text = x.CityName }).ToList();
            ViewBag.Sehir = sehir;
            return View(yemekListesi);
        }
        [HttpPost]
        public IActionResult YemekListesi(string Şehir = "Sivas")
        {
            var yemeklistesi = _db.YemekListesi.Include(x => x.City).OrderBy(x => x.Day).Where(x => x.City.CityName == Şehir & x.Day.Month == DateTime.Now.Month &&
                x.Day.Year == DateTime.Now.Year).ToList();
            var sehir = _db.Cities.Select(x => new SelectListItem
            {
                Value = x.CityName,
                Text = x.CityName,
                Selected = x.CityName == Şehir
            }).ToList();
            ViewBag.Sehir = sehir;
            return View(yemeklistesi);
        }
        [HttpGet]
        public IActionResult YemekListesiDüzenle(int id)
        {
            var sehir = _db.Cities.Select(x => new SelectListItem
            {
                Value = x.CityName,
                Text = x.CityName,


            }).ToList();
            ViewBag.Sehir = sehir;

            var yemek = _db.YemekListesi.Where(x => x.Id == id).SingleOrDefault();
            return View(yemek);
        }
        [HttpPost]
        public IActionResult YemekListesiDüzenle(Yemek yemek)
        {
            var dbyemek = _db.YemekListesi.Find(yemek.Id);
            if (dbyemek != null)
            {
                dbyemek.City = yemek.City; // 
                dbyemek.SabahYemekListesi = yemek.SabahYemekListesi;
                dbyemek.AksamYemekListesi = yemek.AksamYemekListesi;
                dbyemek.SabahCalorie = yemek.SabahCalorie;
                dbyemek.AksamCaloriee = yemek.AksamCaloriee;
                _db.SaveChanges();
            }
            return RedirectToAction("YemekListesi");
        }

        public IActionResult YemekListesiSil(int id)

        {
            var silinecekyemeklistesi = _db.YemekListesi.Find(id);
            _db.Remove(silinecekyemeklistesi);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public IActionResult ManageAdmins()
        {
            var admins = _db.Admins.ToList();
            return View(admins);
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult AddAdmin()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult AddAdmin(Admin newAdmin)
        {
            if (ModelState.IsValid)
            {
                _db.Admins.Add(newAdmin);  // Yeni admini veritabanına ekle
                _db.SaveChanges();
                return RedirectToAction("ManageAdmins");
            }
            return View(newAdmin);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult EditAdmin(int id)
        {
            var editadmin = _db.Admins.Find(id);
            if (editadmin == null)
            {
                return RedirectToAction("ManageAdmins");
            }
            return View(editadmin);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult EditAdmin(Admin admin)
        {

            var editadmin = _db.Admins.SingleOrDefault(x => x.Id == admin.Id);
            if (editadmin != null)
            {
                editadmin.AdminName = admin.AdminName;
                editadmin.AdminRole = admin.AdminRole;
                editadmin.AdminPassword = admin.AdminPassword;
                _db.SaveChanges();
                return RedirectToAction("ManageAdmins");
            }
            return View(admin);
        }
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult DeleteAdmin(int id)
        {
            var deleteadmin = _db.Admins.Find(id);
            if (deleteadmin != null)
            {
                _db.Remove(deleteadmin);
                _db.SaveChanges();
                TempData["SuccesDeleteMessage"] = "Silme İşlemi Başarılı Admin Silindi!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Silme İşlemi Başarısız";
                return RedirectToAction("ManageAdmins");

            }
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() { return View(); }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Admin admin)
        {

            var info = _db.Admins.FirstOrDefault(x => x.AdminName == admin.AdminName && x.AdminPassword == admin.AdminPassword);

            if (info != null)
            {
                var claims = new List<Claim>
                {
                    new  Claim(ClaimTypes.Name,info.AdminName),
                    new Claim(ClaimTypes.Role,info.AdminRole.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Admin");
            }

            else
            {
                ViewBag.ErrorMessage = "Kullanıcı Adı Şifre";
                return View();
            }




        }

        public async Task<IActionResult> Logut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Admin");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

         
    }
}
