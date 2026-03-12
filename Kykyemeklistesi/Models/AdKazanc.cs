using System;
using System.ComponentModel.DataAnnotations;

namespace Kykyemeklistesi.Models
{
    public class AdKazanc
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tarih")]
        [DataType(DataType.Date)]
        public DateTime Tarih { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Miktar (TL)")]
        [Range(0, 1000000, ErrorMessage = "Lütfen geçerli bir miktar giriniz.")]
        public decimal Miktar { get; set; }

        [Required]
        [Display(Name = "Reklam Ağı")]
        public string ReklamAgi { get; set; } = "AdSense"; // AdSense, Yandex vb.

        [Display(Name = "Tıklama Sayısı")]
        public int TiklamaSayisi { get; set; }

        [Display(Name = "Görüntüleme")]
        public int Goruntuleme { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.Now;
    }
}
