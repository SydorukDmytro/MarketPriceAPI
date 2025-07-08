using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPriceAPI.Models
{
    public class Profile
    {
        public string? Name { get; set; }
        [NotMapped]
        public object? Gics { get; set; }
    }
}
