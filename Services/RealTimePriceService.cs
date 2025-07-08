using MarketPriceAPI.Data;
using MarketPriceAPI.Models;
using MarketPriceAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketPriceAPI.Services
{
    public class RealTimePriceService : IRealTimePriceService
    {
        private readonly MarketPriceDbContext _dbContext;

        public RealTimePriceService(MarketPriceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RealTimePriceSnapshot> GetLatestPriceAsync(Guid instrumentId)
        {
            return await _dbContext.RealTimePriceSnapshots
                .Where(r => r.instrumentId.Equals(instrumentId))
                .OrderByDescending(r => r.Timestamp)
                .FirstOrDefaultAsync();
        }
    }
}
