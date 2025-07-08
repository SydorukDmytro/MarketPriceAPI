using System.Text.Json.Serialization;

namespace MarketPriceAPI.DTO
{
    public class PriceInfo
    {
        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; set; }

        [JsonPropertyName("price")]
        public decimal? Price { get; set; }

        [JsonPropertyName("volume")]
        public int? Volume { get; set; }
    }
}
