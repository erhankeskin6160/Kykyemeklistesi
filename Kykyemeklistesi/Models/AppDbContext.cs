using Microsoft.EntityFrameworkCore;

namespace Kykyemeklistesi.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext()
        {
                
        }
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {
                
        }
       public DbSet<Yemek> YemekListesi { get; set; }
    }
}
