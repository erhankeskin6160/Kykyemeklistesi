using System.ComponentModel.DataAnnotations.Schema;

namespace Kykyemeklistesi.Models
{
    public class City
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public List<Yemek> Yemeks { get; set; } = new List<Yemek>();
    }
}
