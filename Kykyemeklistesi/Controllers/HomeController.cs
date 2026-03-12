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

            var selectList = _dbContext.YemekListesi.Where(x => x.Day.Month == DateTime.Now.Month &&
                x.Day.Year == DateTime.Now.Year && x.SabahYemekListesi != null)
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

            // SEO Meta Bilgileri
            ViewData["Title"] = $"{selectedCity} KYK Yemek Listesi {currentDate.Year} | Bugün KYK'da Ne Var?";
            ViewData["Description"] = $"{selectedCity} KYK yurtları {currentDate.ToString("MMMM yyyy")} aylık yemek menüsü. Bugün KYK sabah kahvaltısı ve akşam yemeğinde ne var? Hemen öğrenin.";
            ViewData["Keywords"] = $"{selectedCity} kyk yemek listesi, kyk yemek menüsü {currentDate.Year}, {selectedCity} yurt yemekleri";
            ViewData["CanonicalUrl"] = $"https://kykyemeklistesi.com.tr/{selectedCity.ToLower()}-yemek-listesi";

            deger = selectedCity;

            // Oylama sayılarını yükle
            foreach (var y in yemekListesi)
            {
                y.LikeCount = _dbContext.MenuOylari.Count(o => o.YemekId == y.Id && o.IsLike);
                y.DislikeCount = _dbContext.MenuOylari.Count(o => o.YemekId == y.Id && !o.IsLike);
            }

            return View(yemekListesi);
        }

        public IActionResult Bugunyemeklistesi(string? selectedCity = "Ankara")
        {

            selectedCity = NormalizeCityName(selectedCity);
            if (!Request.Path.Value.Contains($"{selectedCity}-bugun-yemek-listesi") &&
        !Request.Path.Value.Contains("/Home/Index"))
            {
                return RedirectToAction("Bugunyemeklistesi", new { selectedCity });
            }

            var selectList = _dbContext.YemekListesi.Where(x => x.Day.Month == DateTime.Now.Month &&
               x.Day.Year == DateTime.Now.Year && x.SabahYemekListesi != null)
               .GroupBy(x => x.City.CityName)
               .Select(x => new SelectListItem
               {
                   Value = x.Key,
                   Text = x.Key,
                   Selected = x.Key == selectedCity
               }).ToList();

            ViewBag.City = selectList;
            ViewBag.SelectedCity = selectedCity;

            // SEO Meta Bilgileri
            ViewData["Title"] = $"Bugün {selectedCity} KYK'da Ne Yemek Var? | {DateTime.Now:dd MMMM yyyy}";
            ViewData["Description"] = $"{selectedCity} KYK yurtları {DateTime.Now:dd MMMM yyyy} tarihli en güncel yemek listesi. Kahvaltı ve akşam yemeğinde neler var?";
            ViewData["Keywords"] = "kyk bugün ne yemek var, kyk yemek menüsü bugün, güncel kyk listesi";
            ViewData["CanonicalUrl"] = $"https://kykyemeklistesi.com.tr/{selectedCity.ToLower()}-bugun-yemek-listesi";

            var bugunyemeklistesi = _dbContext.YemekListesi.Include(x => x.City).OrderBy(x => x.Day)
                .Where(x => x.City.CityName == selectedCity && x.Day == DateTime.Now.Date)
                .ToList();

            // Oylama sayılarını yükle
            foreach (var y in bugunyemeklistesi)
            {
                y.LikeCount = _dbContext.MenuOylari.Count(o => o.YemekId == y.Id && o.IsLike);
                y.DislikeCount = _dbContext.MenuOylari.Count(o => o.YemekId == y.Id && !o.IsLike);
            }

            return View(bugunyemeklistesi);
        }

        //[HttpPost]
        //public IActionResult YorumEkle(YorumDto yorumDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Json(new { success = false, message = "Lütfen tüm alanları doldurun." });
        //    }

        //    // Rate limiting - Aynı IP'den 10 dakikada bir yorum
        //    var clientIp = GetClientIpAddress();
        //    var sonYorum = _dbContext.YemekYorumlari
        //        .Where(y => y.IpAdresi == clientIp)
        //        .OrderByDescending(y => y.YorumTarihi)
        //        .FirstOrDefault();

        //    if (sonYorum != null && (DateTime.Now - sonYorum.YorumTarihi).TotalMinutes < 10)
        //    {
        //        return Json(new { success = false, message = "Çok sık yorum yapıyorsunuz. Lütfen bekleyin." });
        //    }

        //    // Küfür filtresi
        //    if (KufurKontrolu(yorumDto.YorumMetni))
        //    {
        //        return Json(new { success = false, message = "Yorumunuz uygunsuz içerik barındırıyor." });
        //    }

        //    var yorum = new YemekYorum
        //    {
        //        YemekId = yorumDto.YemekId,
        //        OgrenciAdi = yorumDto.OgrenciAdi.Trim(),
        //         YorumMetni = yorumDto.YorumMetni.Trim(),
        //        Puan = yorumDto.Puan,
        //        YorumTarihi = DateTime.Now,
        //        IpAdresi = clientIp,
        //        OnayDurumu = true // Otomatik onay, isterseniz false yapıp manuel onay sistemi kurabilirsiniz
        //    };

        //    _dbContext.YemekYorumlari.Add(yorum);
        //    _dbContext.SaveChanges();

        //    return Json(new { success = true, message = "Yorumunuz başarıyla eklendi!" });
        //}


        //public IActionResult YorumlariGetir(int yemekId, int sayfa = 1)
        //{
        //    const int sayfaBasinaYorum = 5;

        //    var yorumlar = _dbContext.YemekYorumlari
        //        .Where(y => y.YemekId == yemekId && y.OnayDurumu)
        //        .OrderByDescending(y => y.YorumTarihi)
        //        .Skip((sayfa - 1) * sayfaBasinaYorum)
        //        .Take(sayfaBasinaYorum)
        //        .Select(y => new {
        //            y.OgrenciAdi,
        //            y.YorumMetni,
        //            y.Puan,
        //            YorumTarihi = y.YorumTarihi.ToString("dd.MM.yyyy HH:mm")
        //        })
        //        .ToList();

        //    var toplamYorum = _dbContext.YemekYorumlari
        //        .Count(y => y.YemekId == yemekId && y.OnayDurumu);

        //    return Json(new
        //    {
        //        yorumlar,
        //        toplamYorum,
        //        toplamSayfa = (int)Math.Ceiling((double)toplamYorum / sayfaBasinaYorum),
        //        mevcutSayfa = sayfa
        //    });
        //}



        private string GetClientIpAddress()
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress;
            if (ipAddress != null)
            {
                return ipAddress.ToString();
            }
            return "Unknown";
        }

        private bool KufurKontrolu(string metin)
        {
            var kufurListesi = new[] { "küfür1", "küfür2", "uygunsuz1" }; // Gerçek listesini ekleyin
            return kufurListesi.Any(kufur =>
                metin.ToLowerInvariant().Contains(kufur.ToLowerInvariant()));
        }

        // Şehir adını normalize eden helper metot
        private string NormalizeCityName(string cityName)
        {
            if (string.IsNullOrEmpty(cityName))
                return "Ankara";


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

        public IActionResult SSS()
        {
            return View();
        }



        public IActionResult Static(string? selectedCity = "Ankara")
        {

            var selectList = _dbContext.YemekListesi.Where(x => x.Day.Month == DateTime.Now.Month &&
               x.Day.Year == DateTime.Now.Year && x.SabahYemekListesi != null)
               .GroupBy(x => x.City.CityName)
               .Select(x => new SelectListItem
               {
                   Value = x.Key,
                   Text = x.Key,
                   Selected = x.Key == selectedCity
               }).ToList();
            ViewBag.SelectedCity = selectList;
            ViewBag.secilen = selectedCity;


            // 1. Veritabanından tüm sabah listelerini string olarak çektiniz (Bu satır doğru).
            // 1. Veritabanından tüm sabah listelerini string olarak çektiniz.
            var sabahyemeklistesi = _dbContext.YemekListesi.Where(x => x.City.CityName == selectedCity && x.Day.Year == DateTime.Now.Year && x.Day.Month == DateTime.Now.Month).Select(x => x.SabahYemekListesi).ToList();

            // YENİ ADIM: Sayıma dahil edilmeyecek, filtrelenecek ürünlerin listesini tanımla.
            // Bu listeyi istediğiniz gibi genişletebilirsiniz.
            var filtrelenecekUrunler = new HashSet<string>
{
    "Çeyrek Ekmek",
    "500 ml Su",
     "500 mı su",
     "",
     "Soslu"
    // Buraya başka istenmeyen ürünler ekleyebilirsiniz, örneğin: "Tuz", "Peçete"
};


            var Sabahyemeğilist5 = sabahyemeklistesi
                .Where(metin => !string.IsNullOrEmpty(metin))
                .SelectMany(metin => metin.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(urun => urun.Trim())
                .Where(urun => !filtrelenecekUrunler.Contains(urun))
                .GroupBy(urun => urun)
                .OrderByDescending(grup => grup.Count())
                .Take(5) // İlk 5'i al
                .Select(grup => new
                {
                    UrunAdi = grup.Key,
                    TekrarSayisi = grup.Count()
                })
                .ToList();

            ViewBag.sabahyemeği = Sabahyemeğilist5;
            var aksamyemeklistesi = _dbContext.YemekListesi.Where(x => x.City.CityName == selectedCity && x.Day.Year == DateTime.Now.Year && x.Day.Month == DateTime.Now.Month).Select(x => x.AksamYemekListesi).ToList();



            var enCokCikanCorbalar = aksamyemeklistesi
                .Where(x => !string.IsNullOrEmpty(x))
                .SelectMany(yemekMetni => yemekMetni.Split(new[] { '/', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(yemek => yemek.Trim())
                .Where(yemek => yemek.Contains("Çorba", StringComparison.OrdinalIgnoreCase))
                .Where(corba => !filtrelenecekUrunler.Contains(corba))
                .GroupBy(corba => corba)
                .OrderByDescending(grup => grup.Count())
                .Take(5)
                .Select(grup => new { CorbaAdi = grup.Key, TekrarSayisi = grup.Count() })
                .ToList();

            ViewBag.enCokCikanCorbalar = enCokCikanCorbalar;

            // Veritabanından çekilmiş menü listesi
            // List<string> aksamyemeklistesi = ...;

            var den = aksamyemeklistesi
    //---------- ADIM 1: Ana Yemekleri Çekme ve Ayrıştırma Bölümü ----------
    .Where(metin => !string.IsNullOrEmpty(metin))
    .Select(metin => metin.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
    .Where(satirlar => satirlar.Length > 1)
    .Select(satirlar => satirlar[1])
    .SelectMany(anaYemekSatiri => anaYemekSatiri.Split('/'))
    .Select(anaYemek => anaYemek.Trim()).ToList();

            // İki adımı birleştiren tek ve akıcı LINQ sorgusu
            var enCokCikanAnaYemekler = aksamyemeklistesi
    //---------- ADIM 1: Ana Yemekleri Çekme ve Ayrıştırma Bölümü ----------
    .Where(metin => !string.IsNullOrEmpty(metin))
    .Select(metin => metin.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
    .Where(satirlar => satirlar.Length > 1)
    .Select(satirlar => satirlar[1])
    .SelectMany(anaYemekSatiri => anaYemekSatiri.Split('/'))
    .Select(anaYemek => anaYemek.Trim())

    //---------- YENİ FİLTRELEME ADIMI ----------
    // Yukarıda tanımlanan `filtrelenecekUrunler` listesinde OLMAYAN yemeklerle devam et.
    .Where(yemek => !filtrelenecekUrunler.Contains(yemek))

    //---------- ADIM 2: Gruplama, Sıralama ve Seçme Bölümü ----------
    .GroupBy(yemek => yemek)
    .OrderByDescending(grup => grup.Count())
    .Take(5) // İlk 5'i al
    .Select(grup => new
    {
        AnaYemekAdi = grup.Key,
        TekrarSayisi = grup.Count()
    })
    .ToList();
            ViewBag.Anayemek = enCokCikanAnaYemekler;

            // SEO Meta Bilgileri
            ViewData["Title"] = $"{selectedCity} KYK Yemek İstatistikleri ve Kalori Analizi";
            ViewData["Description"] = $"{selectedCity} KYK yurt yemekleri kalori raporu, besin değerleri ve aylık en çok çıkan yemeklerin istatistik analizi.";
            ViewData["Keywords"] = "kyk yemek kalori, kyk besin değerleri, kyk yemek istatistikleri, yemek analizi";
            ViewData["CanonicalUrl"] = $"https://kykyemeklistesi.com.tr/Home/Static?selectedCity={selectedCity}";

            return View();
        }

        [HttpGet]
        public IActionResult Paylas()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Paylas(MenuPaylasim model, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    // Dosya uzantısı kontrolü
                    var extension = Path.GetExtension(file.FileName).ToLower();
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf", ".xlsx", ".xls" };
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("file", "Sadece Resim (JPG, PNG), PDF veya Excel dosyaları yüklenebilir.");
                        return View(model);
                    }

                    // Dosya boyutu kontrolü (max 10MB)
                    if (file.Length > 10 * 1024 * 1024)
                    {
                        ModelState.AddModelError("file", "Dosya boyutu 10MB'dan küçük olmalıdır.");
                        return View(model);
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        model.DosyaVerisi = memoryStream.ToArray();
                    }
                    
                    model.DosyaUzantisi = extension;
                    model.DosyaYolu = file.FileName; // Orijinal adını referans olarak tutalım
                }
                else
                {
                    ModelState.AddModelError("file", "Lütfen bir yemek listesi dosyası seçiniz.");
                    return View(model);
                }

                _dbContext.MenuPaylasimlar.Add(model);
                await _dbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Yemek listeniz başarıyla gönderildi! İncelemenin ardından sisteme dahil edilecektir. Desteğiniz için teşekkürler.";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Oyla(int yemekId, bool isLike)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ip)) ip = "unknown";

            // Bugün bu IP ile bu yemeğe oy verilmiş mi?
            var existingVote = await _dbContext.MenuOylari
                .FirstOrDefaultAsync(o => o.YemekId == yemekId && o.IpAdresi == ip && o.OyTarihi.Date == DateTime.Now.Date);

            if (existingVote != null)
            {
                var likesCurrent = await _dbContext.MenuOylari.CountAsync(o => o.YemekId == yemekId && o.IsLike);
                var dislikesCurrent = await _dbContext.MenuOylari.CountAsync(o => o.YemekId == yemekId && !o.IsLike);
                return Json(new { success = false, message = "Bugün bu menü için zaten oy kullandınız.", likes = likesCurrent, dislikes = dislikesCurrent });
            }

            var yeniOy = new MenuOy
            {
                YemekId = yemekId,
                IpAdresi = ip,
                IsLike = isLike
            };

            _dbContext.MenuOylari.Add(yeniOy);
            await _dbContext.SaveChangesAsync();

            // Güncel sayıları alalım
            var likes = await _dbContext.MenuOylari.CountAsync(o => o.YemekId == yemekId && o.IsLike);
            var dislikes = await _dbContext.MenuOylari.CountAsync(o => o.YemekId == yemekId && !o.IsLike);

            return Json(new { success = true, likes = likes, dislikes = dislikes });
        }

        public static string SecilenSehir()
        {
            var secilensehir = deger;
            return secilensehir;
        }
    }
}