using System;
using System.ComponentModel.DataAnnotations;

namespace Kykyemeklistesi.Models
{
    public class MenuOy
    {
        public int Id { get; set; }
        
        [Required]
        public int YemekId { get; set; }
        
        [Required]
        public string IpAdresi { get; set; }
        
        public bool IsLike { get; set; } // true: Beğendi, false: Beğenmedi
        
        public DateTime OyTarihi { get; set; } = DateTime.Now;
        
        // Navigation property
        public virtual Yemek Yemek { get; set; }
    }
}
