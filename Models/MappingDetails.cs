namespace MarketPriceAPI.Models
{
    public class MappingDetails
    {
        public bool IsPresent { get; set; } = true;
        public String Symbol { get; set; }
        public string? Exchange { get; set; }
        public int? DefaultOrderSize { get; set; }
        public int? MaxOrderSize { get; set; }
        public TradingHours TradingHours { get; set; }
    }
}
