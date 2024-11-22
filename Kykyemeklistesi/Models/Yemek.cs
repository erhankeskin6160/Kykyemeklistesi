namespace Kykyemeklistesi.Models
{
    public class Yemek
    {
        public int Id { get; set; }
        public DateTime Day { get; set; } = DateTime.Now;

        public string City { get; set; }
         
      public  string? SabahYemekListesi { get; set; }
        public string? AksamYemekListesi { get; set; }
        public double Calorie { get; set; }
 
    }

   
}
