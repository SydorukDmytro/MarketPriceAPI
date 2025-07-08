using System.Text.Json.Serialization;

namespace MarketPriceAPI.DTO
{
    public class RealtimePriceDto
    {
        [JsonPropertyName("instrumentId")]
        public string InstrumentId { get; set; }

        [JsonPropertyName("ask")]
        public PriceInfo Ask { get; set; }

        [JsonPropertyName("bid")]
        public PriceInfo Bid { get; set; }

        [JsonPropertyName("last")]
        public PriceInfo Last { get; set; }
    }

}
