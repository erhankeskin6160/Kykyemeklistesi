using Kykyemeklistesi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Kykyemeklistesi.Controllers
{
    public class HomeController : Controller
    {
        private static AppDbContext _dbContext;

        public HomeController(AppDbContext appContext)
        {
            _dbContext = appContext;
        }



        [HttpGet]
        public IActionResult Index(string? selectedCity = "Sivas")
        {
            DateTime currentDate = DateTime.Now;


            // �ehirler i�in SelectList

            var selectList = _dbContext.YemekListesi


                .GroupBy(x => x.City.CityName)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Key,
                    Selected = x.Key == selectedCity
                }).ToList();

            // Se�ilen �ehre g�re yemek listesi
            var yemekListesi = _dbContext.YemekListesi.Include(x => x.City).OrderBy(x => x.Day)
                .Where(x => x.City.CityName == selectedCity &&
                x.Day.Month == DateTime.Now.Month &&
                x.Day.Year == DateTime.Now.Year)
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


        public IActionResult Bugunyemeklistesi(string? selectedCity = "Ankara")
        {
            var selectList = _dbContext.YemekListesi


               .GroupBy(x => x.City.CityName)
               .Select(x => new SelectListItem
               {
                   Value = x.Key,
                   Text = x.Key,
                   Selected = x.Key == selectedCity
               }).ToList();

            ViewBag.City = selectList;

            var bugunyemeklistesi = _dbContext.YemekListesi.Include(x => x.City).OrderBy(x => x.Day)
                .Where(x => x.City.CityName == selectedCity && x.Day == DateTime.Now.Date)
                .ToList();

            return View(bugunyemeklistesi);
        }


    }


}




