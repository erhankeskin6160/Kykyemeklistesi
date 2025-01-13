using Kykyemeklistesi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using NuGet.Protocol;
using System.IO;
using DocumentFormat.OpenXml.Vml.Wordprocessing;
using iText.Kernel.Font;
using iText.IO.Font;

namespace Kykyemeklistesi.Controllers
{


    public class UserController : Controller
    {
        AppDbContext _db;
        static bool giris = false;
        public UserController(AppDbContext appDbContext)
        {
            _db = appDbContext;
            
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();



        }
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {

            var info = _db.Users.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);

            if (info != null)
            {
                var claims = new List<Claim>
                {
                    new  Claim(ClaimTypes.Email,info.Email),
                    
                 };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
              

                return RedirectToAction("Index", "User");
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
            return RedirectToAction("Login", "User");
        }



        public IActionResult ExportToExcel()
        {
            var info = _db.YemekListesi.Include(x => x.City)
                .Where(x => x.City.CityName == HomeController.SecilenSehir())
                .OrderBy(x => x.Day)
                .ToList();
            using (var excel = new XLWorkbook())
            {
                var work = excel.Worksheets.Add("YemekListesi");
                var currentrow = 1;

                work.Cell(currentrow, 1).Value = "Tarih";
                work.Cell(currentrow, 2).Value = "SabahYemekListesi";
                work.Cell(currentrow, 3).Value = "AksamYemekListesi";

                foreach (var item in info)
                {
                    currentrow++;

                    work.Cell(currentrow, 1).Value = item.Day;
                    work.Cell(currentrow, 2).Value = item.SabahYemekListesi;
                    work.Cell(currentrow, 3).Value = item.AksamYemekListesi;
                }

                using (var stream = new System.IO.MemoryStream())
                {
                    excel.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "KykYemekListesi.xlsx");

                }
            }

        }


        public IActionResult ExportToPdf()
        {
            // Seçilen şehre göre YemekListesi verilerini getirin
            var info = _db.YemekListesi.Include(x => x.City)
                .Where(x => x.City.CityName == HomeController.SecilenSehir())
                .OrderBy(x => x.Day)
                .ToList();

            // Türkçe karakterleri destekleyen font yolu
            var fontPath = "wwwroot/font/DejaVuSans.ttf"; // DejaVuSans veya Türkçe karakterleri destekleyen bir font
            var font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

            using (var pdf = new System.IO.MemoryStream())
            {
                // PDF Writer ve Document oluştur
                var writer = new iText.Kernel.Pdf.PdfWriter(pdf);
                var pdfDocument = new iText.Kernel.Pdf.PdfDocument(writer);
                var document = new iText.Layout.Document(pdfDocument);

                // Başlık ekle
                var baslik = new iText.Layout.Element.Paragraph(HomeController.SecilenSehir() + " Yemek Listesi")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontSize(18)
                    .SetFont(font); // Başlık için font ayarlandı
                document.Add(baslik);

                // Boşluk eklemek için paragraf
                document.Add(new iText.Layout.Element.Paragraph("\n"));

                // Tablo oluştur ve başlıklar ekle
                var table = new iText.Layout.Element.Table(3).UseAllAvailableWidth();
                table.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Tarih").SetFont(font)));
                table.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Sabah Yemek Listesi").SetFont(font)));
                table.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Akşam Yemek Listesi").SetFont(font)));

                // Verileri tabloya ekle
                foreach (var item in info)
                {
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(item.Day.ToString("dd.MM.yyyy")).SetFont(font))); // Tarih
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(item.SabahYemekListesi ?? "-").SetFont(font)));    // Sabah yemeği
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(item.AksamYemekListesi ?? "-").SetFont(font)));    // Akşam yemeği
                }
 
             
                // Tabloyu döküman içine ekle
                document.Add(table);

                // PDF dökümanını kapat
                document.Close();

                // PDF içeriğini indirme için döndür
                var content = pdf.ToArray();
                return File(content, "application/pdf", "YemekListesi.pdf");
            }
        }

    }


}





