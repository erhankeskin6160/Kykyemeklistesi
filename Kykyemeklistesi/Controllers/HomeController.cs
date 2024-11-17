using Kykyemeklistesi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kykyemeklistesi.Controllers
{
    public class HomeController : Controller
    {

        AppDbContext dbContext= new AppDbContext();
        public HomeController(AppDbContext appContext )
        {
                dbContext = appContext;
        }


        public IActionResult Index()
        {
            dbContext.YemekListesi.ToList();
            return View();
        }


    }
}
