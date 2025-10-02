namespace Kykyemeklistesi.Models
{
    public class YemekYorum
    {
        public int Id { get; set; }
        public int YemekId { get; set; }
        public string OgrenciAdi { get; set; }
   
        public string YorumMetni { get; set; }
        public int Puan { get; set; } // 1-5 arası değerlendirme
        public DateTime YorumTarihi { get; set; }
        public bool OnayDurumu { get; set; } = false; // Moderasyon için
        public string IpAdresi { get; set; }

        // Navigation property
        public virtual Yemek Yemek { get; set; }
    }

    public class YorumDto
    {
        public string OgrenciAdi { get; set; }
 
        public string YorumMetni { get; set; }
        public int Puan { get; set; }
        public int YemekId { get; set; }
    }

}
