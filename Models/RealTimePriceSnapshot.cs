using System.ComponentModel.DataAnnotations;

namespace MarketPriceAPI.Models
{
    public class RealTimePriceSnapshot
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid instrumentId { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal Last { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
