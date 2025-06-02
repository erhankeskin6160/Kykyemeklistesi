// HomeController.cs - Minimal değişikliklerle
using Kykyemeklistesi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Xml;

namespace Kykyemeklistesi.Controllers
{
    public class HomeController : Controller
    {
        private static AppDbContext _dbContext;

        public HomeController(AppDbContext appContext)
        {
            _dbContext = appContext;
        }
        static string deger;

        [HttpGet]
        public IActionResult Index(string? selectedCity = "Ankara")
        {
            
            selectedCity = NormalizeCityName(selectedCity);

            if (!Request.Path.Value.Contains($"{selectedCity}-yemek-listesi") && 
        !Request.Path.Value.Contains("/Home/Bugunyemeklistesi"))
            {
                return RedirectToAction("Index", new { selectedCity });
            }
            DateTime currentDate = DateTime.Now;

            var selectList = _dbContext.YemekListesi
                .GroupBy(x => x.City.CityName)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Key,
                    Selected = x.Key == selectedCity
                }).ToList();

            var yemekListesi = _dbContext.YemekListesi.Include(x => x.City).OrderBy(x => x.Day)
                .Where(x => x.City.CityName == selectedCity &&
                x.Day.Month == DateTime.Now.Month &&
                x.Day.Year == DateTime.Now.Year)
                .ToList();

            ViewBag.City = selectList;
            ViewBag.SelectedCity = selectedCity;

            deger = selectedCity;
            return View(yemekListesi);
        }

        public IActionResult Bugunyemeklistesi(string? selectedCity = "Ankara" )
        {
           
            selectedCity = NormalizeCityName(selectedCity);
            if (!Request.Path.Value.Contains($"{selectedCity}-bugun-yemek-listesi") &&   
        !Request.Path.Value.Contains("/Home/Index")) 
            {
                return RedirectToAction("Bugunyemeklistesi", new { selectedCity });
            }

            var selectList = _dbContext.YemekListesi
               .GroupBy(x => x.City.CityName)
               .Select(x => new SelectListItem
               {
                   Value = x.Key,
                   Text = x.Key,
                   Selected = x.Key == selectedCity
               }).ToList();

            ViewBag.City = selectList;
            ViewBag.SelectedCity = selectedCity;

            var bugunyemeklistesi = _dbContext.YemekListesi.Include(x => x.City).OrderBy(x => x.Day)
                .Where(x => x.City.CityName == selectedCity && x.Day == DateTime.Now.Date)
                .ToList();

            return View(bugunyemeklistesi);
        }

        // Şehir adını normalize eden helper metot
        private string NormalizeCityName(string cityName)
        {
            if (string.IsNullOrEmpty(cityName))
                return "Ankara";

            // İlk harfi büyük yap, gerisini küçük yap
            return char.ToUpper(cityName[0]) + cityName.Substring(1).ToLower();
        }

        public IActionResult GenerateSitemap()
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlElement urlSetElement = xmlDoc.CreateElement("urlset");
            urlSetElement.SetAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
            urlSetElement.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            urlSetElement.SetAttribute("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/siteindex.xsd");

            xmlDoc.AppendChild(urlSetElement);

            var cities = _dbContext.Cities.ToList();

            foreach (var city in cities)
            {
                // SEO dostu URL'leri sitemap'e ekliyoruz
                string cityUrlSeoFriendly = $"https://kykyemeklistesi.com.tr/{city.CityName.ToLower()}-yemek-listesi";
                string todayMenuUrlSeoFriendly = $"https://kykyemeklistesi.com.tr/{city.CityName.ToLower()}-bugun-yemek-listesi";

                // Eski URL'leri de ekliyoruz (geriye uyumluluk için)
                string cityUrl = $"https://kykyemeklistesi.com.tr/Home/Index?selectedCity={city.CityName}";
                string todayMenuUrl = $"https://kykyemeklistesi.com.tr/Home/Bugunyemeklistesi?selectedCity={city.CityName}";

                // SEO dostu şehir menü URL'i
                XmlElement cityUrlElement = xmlDoc.CreateElement("url");
                XmlElement cityLoc = xmlDoc.CreateElement("loc");
                cityLoc.InnerText = cityUrlSeoFriendly;
                cityUrlElement.AppendChild(cityLoc);

                XmlElement cityLastMod = xmlDoc.CreateElement("lastmod");
                cityLastMod.InnerText = DateTime.UtcNow.ToString("yyyy-MM-dd");
                cityUrlElement.AppendChild(cityLastMod);

                XmlElement cityChangeFreq = xmlDoc.CreateElement("changefreq");
                cityChangeFreq.InnerText = "daily";
                cityUrlElement.AppendChild(cityChangeFreq);

                XmlElement cityPriority = xmlDoc.CreateElement("priority");
                cityPriority.InnerText = "0.8";
                cityUrlElement.AppendChild(cityPriority);

                urlSetElement.AppendChild(cityUrlElement);

                // SEO dostu bugünkü menü URL'i
                XmlElement todayMenuUrlElement = xmlDoc.CreateElement("url");
                XmlElement todayMenuLoc = xmlDoc.CreateElement("loc");
                todayMenuLoc.InnerText = todayMenuUrlSeoFriendly;
                todayMenuUrlElement.AppendChild(todayMenuLoc);

                XmlElement todayMenuLastMod = xmlDoc.CreateElement("lastmod");
                todayMenuLastMod.InnerText = DateTime.UtcNow.ToString("yyyy-MM-dd");
                todayMenuUrlElement.AppendChild(todayMenuLastMod);

                XmlElement todayMenuChangeFreq = xmlDoc.CreateElement("changefreq");
                todayMenuChangeFreq.InnerText = "daily";
                todayMenuUrlElement.AppendChild(todayMenuChangeFreq);

                XmlElement todayMenuPriority = xmlDoc.CreateElement("priority");
                todayMenuPriority.InnerText = "0.9"; // Bugünkü menü daha yüksek öncelik
                todayMenuUrlElement.AppendChild(todayMenuPriority);

                urlSetElement.AppendChild(todayMenuUrlElement);
            }

            using (var stringWriter = new StringWriter())
            {
                xmlDoc.Save(stringWriter);
                string xmlContent = stringWriter.ToString();

                return Content(xmlContent, "text/xml", Encoding.UTF8);
            }
        }

        public IActionResult NotFound()
        {
            return View();
        }

        public IActionResult SSS() { return View(); }

        public static string SecilenSehir()
        {
            var secilensehir = deger;
            return secilensehir;
        }
    }
}