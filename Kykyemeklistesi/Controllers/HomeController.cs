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
            var data = dbContext.YemekListesi.ToList();
            List<SelectListItem> selectListItem = dbContext.YemekListesi.GroupBy(x => x.City).Select(x => new SelectListItem
            {
                Value = x.Key,
                Text = x.Key,
            }
           ).ToList();

            ViewBag.City = selectListItem;
            return View(data);
        }



        [HttpPost]
        public IActionResult Index(string ?selectedlist="Ankara")
        {
            List<SelectListItem> selectListItem = dbContext.YemekListesi.GroupBy(x => x.City).Select(x => new SelectListItem
            {
                Value = x.Key,
                Text=x.Key,
            }
            ).ToList();

            ViewBag.City= selectListItem;
            var yemeklistesi = string.IsNullOrEmpty(selectedlist) ? dbContext.YemekListesi.ToList() : dbContext.YemekListesi.Where(x => x.City == selectedlist).ToList();
            return View(yemeklistesi);

        
        
        
        }
       

    }
}
