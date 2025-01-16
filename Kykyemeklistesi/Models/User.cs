namespace Kykyemeklistesi.Models
{
    public class User
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public string ?Soyad { get; set; }
        public string Email{ get; set; }
        
        public string Password{ get; set; }


        public string? University { get; set; }
        public string? Department { get; set; }
        public string? Faculty { get; set; }
        public string? City { get; set; }

        public string? Resim { get; set; } ="erkek.png";

        public Gender? Gender { get; set; } =0;
       



    }
    public enum Gender
    {
        Erkek,
        Kadın
    }
}
