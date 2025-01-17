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
        public DbSet<City> Cities { get; set; }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<Anket> Ankets { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Yemek>()
                .HasOne(y => y.City)
                .WithMany(c => c.Yemeks) // City -> Yemeks ilişkilendirilmiş
                .HasForeignKey(y => y.CityId)
                .OnDelete(DeleteBehavior.Restrict);


            
        }


    }
}
