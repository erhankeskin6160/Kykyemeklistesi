namespace Kykyemeklistesi.Models
{
    public class Yemek
    {
        public int Id { get; set; }
        public int CityId { get; set; } // Şehri temsil eden yabancı anahtar
        public DateTime Day { get; set; } = DateTime.Now.Date;
        public string? SabahYemekListesi { get; set; }
        public string? AksamYemekListesi { get; set; }
        public double Calorie { get; set; }
        public City City { get; set; }
    }
}
