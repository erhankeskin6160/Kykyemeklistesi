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

          

        public DbSet<User> Users { get; set; }

        public DbSet<YemekYorum> YemekYorumlar { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Yemek>()
                .HasOne(y => y.City)
                .WithMany(c => c.Yemeks) // City -> Yemeks ilişkilendirilmiş
                .HasForeignKey(y => y.CityId)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<YemekYorum>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OgrenciAdi).IsRequired().HasMaxLength(100);
                 entity.Property(e => e.YorumMetni).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Puan).IsRequired();
                entity.Property(e => e.YorumTarihi).IsRequired();
                entity.Property(e => e.IpAdresi).HasMaxLength(45);

                // Foreign key relationship
                entity.HasOne(d => d.Yemek)
                      .WithMany()
                      .HasForeignKey(d => d.YemekId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<User>()
              .HasIndex(u => u.Email)
              .IsUnique();
        }
    }
}


 
