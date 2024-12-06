using Kykyemeklistesi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult YemeklListesiDüzenle(int id) 
        {
          var yemek = _db.YemekListesi.Where(x=>x.Id==id).SingleOrDefault();
            return View(yemek);
        }
        [HttpPost]
        public IActionResult YemekListesiDüzenle(Yemek yemek)
        {
            var dbyemek = _db.YemekListesi.Find(yemek.Id);
            if (dbyemek != null)
            {
                dbyemek.City = yemek.City; // Örnek bir alan, yemek verisine göre güncelleyin
                dbyemek.SabahYemekListesi = yemek.SabahYemekListesi; // Yemek ismi gibi başka alanlar eklenebilir
                dbyemek.AksamYemekListesi = yemek.AksamYemekListesi;
                dbyemek.Calorie = yemek.Calorie;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult  YemekListesiSİl(int id) 
        
        {
           var silinecekyemeklistesi= _db.YemekListesi.Find(id);
            _db.Remove(id);
            _db.SaveChanges();
            return RedirectToAction("Index");   
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult ManageAdmins() 
        {
            return View();  
        }

        public IActionResult Login() { return View(); }

       [HttpPost]
       public async Task<IActionResult> Login(Admin admin) 
        {

            var info = _db.Admins.FirstOrDefault(x => x.AdminName == admin.AdminName && x.AdminPassword ==admin.AdminPassword);

            if (info != null)
            {
                var claims = new List<Claim>
                {
                    new  Claim(ClaimTypes.Name,info.AdminName),
                    new Claim(ClaimTypes.Role,info.AdminRole.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


              await  HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index","Admin");
            }

            else 
            {
                ViewBag.ErrorMessage = "Kullanıcı Adı Şifre";
                return View();
            }
        }
    }
}
