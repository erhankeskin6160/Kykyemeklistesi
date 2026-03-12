using System;
using System.ComponentModel.DataAnnotations;

namespace Kykyemeklistesi.Models
{
    public class MenuPaylasim
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Lütfen adınızı giriniz.")]
        [Display(Name = "Ad Soyad")]
        public string GonderenAdi { get; set; }

        [Required(ErrorMessage = "Lütfen şehri belirtiniz.")]
        [Display(Name = "Şehir")]
        public string Sehir { get; set; }

        [Required(ErrorMessage = "Lütfen yurt adını giriniz.")]
        [Display(Name = "Yurt Adı")]
        public string YurtAdi { get; set; }

        [Display(Name = "Dosya Yolu")]
        public string? DosyaYolu { get; set; }

        public byte[]? DosyaVerisi { get; set; }

        public string? DosyaUzantisi { get; set; }

        [Display(Name = "Gönderim Tarihi")]
        public DateTime GonderimTarihi { get; set; } = DateTime.Now;

        [Display(Name = "Durum")]
        public string Durum { get; set; } = "Onay Bekliyor"; // Onay Bekliyor, Onaylandı, Reddedildi
    }
}
