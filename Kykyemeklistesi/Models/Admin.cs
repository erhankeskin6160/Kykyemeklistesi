namespace Kykyemeklistesi.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public AdminRole AdminRole { get; set; }


        public string AdminName {  get; set; }  

        public string AdminPassword {  get; set; }  
    }

    public enum AdminRole 
    {
        SuperAdmin,
        Admin,
        
    }
}
