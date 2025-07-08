namespace MarketPriceAPI.Models
{
    public class Asset
    {
        public Guid Id { get; set; }
        public string? Symbol { get; set; }
        public string? Kind { get; set; }
        public string? Description { get; set; }
        public double TickSize { get; set; }
        public string? Currency { get; set; }
        public string? BaseCurrency { get; set; }

        public Mappings Mappings { get; set; }
        public Profile Profile { get; set; }
    }
}
