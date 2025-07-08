using MarketPriceAPI.Models;

namespace MarketPriceAPI.Services.Interfaces
{
    public interface IHistoricalPriceService
    {
        public Task<IEnumerable<HistoricalPrice>> GetLatestBarsAsync(Guid istrumentId, string? provider = null, int? interval = null, 
            string? periodicity = null, int barsCount = 10);
        public Task<IEnumerable<HistoricalPrice>> GetBarsByDateRangeAsync(Guid instrumentId, DateTime startDate, DateTime? endDate = null, 
            string? provider = null,
            int? interval = null,
            string? periodicity = null);
        public Task<IEnumerable<HistoricalPrice>> GetBarsByTimeBackAsync(Guid instrumentId, TimeSpan timeBack, string? provider = null,
            int? interval = null,
            string? periodicity = null
            );
    }
}
