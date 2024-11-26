using Kykyemeklistesi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kykyemeklistesi.Controllers
{
    public class HomeController : Controller
    {
        private static  AppDbContext _dbContext;

        public HomeController(AppDbContext appContext)
        {
          _dbContext = appContext;
        }

        

        [HttpGet]
        public IActionResult Index(string? selectedCity="Ankara")
        {

            // Þehirler için SelectList

            var selectList = _dbContext.YemekListesi


                .GroupBy(x => x.City)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Key,
                    Selected = x.Key == selectedCity
                }).ToList();

            // Seçilen þehre göre yemek listesi
            var yemekListesi = _dbContext.YemekListesi
                .Where(x => x.City == selectedCity)
                .ToList();

            ViewBag.City = selectList;
            ViewBag.SelectedCity = selectedCity;

            return View(yemekListesi);
        }

        //[HttpPost]
        //public IActionResult Index(string selectedCity, string? a)
        //{
        //    var selectList = _dbContext.YemekListesi
        //           .GroupBy(x => x.City)
        //           .Select(x => new SelectListItem
        //           {
        //               Value = x.Key,
        //               Text = x.Key,
        //               Selected = x.Key == selectedCity
        //           }).ToList();

        //    var yemekListesi = _dbContext.YemekListesi
        //            .Where(x => x.City == selectedCity)
        //            .ToList();

        //    ViewBag.City = selectList;
        //    return View(yemekListesi);
        //}
    }
}
 



 