using MarketPriceAPI.Models;

namespace MarketPriceAPI.Services.Interfaces
{
    public interface IRealTimePriceService
    {
        Task<RealTimePriceSnapshot> GetLatestPriceAsync(Guid instrumentId);
    }
}
