using Kykyemeklistesi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kykyemeklistesi.Controllers
{
    public class HomeController : Controller
    {

        AppDbContext dbContext= new AppDbContext();
        public HomeController(AppDbContext appContext )
        {
                dbContext = appContext;
        }


        [HttpGet]
        public IActionResult Index()
        {
            List<SelectListItem> selectListItems = dbContext.YemekListesi.Distinct().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.City
            }).ToList();

            var city=selectListItems.FirstOrDefault();
            ViewBag.City = city;
            var yemeklistesi = dbContext.YemekListesi.ToList();
            return View(yemeklistesi);
        }


    }
}
