using MarketPriceAPI.Authentication;
using MarketPriceAPI.Configuration;
using MarketPriceAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MarketPriceAPI.Services
{
    public class RealtimePriceBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly RealtimeSubscriptionOptions _options;
        private readonly WebSocketRealTimePriceService _wsService;

        public RealtimePriceBackgroundService(
            IServiceScopeFactory scopeFactory,
            IOptions<RealtimeSubscriptionOptions> options,
            WebSocketRealTimePriceService wsService)
        {
            _scopeFactory = scopeFactory;
            _options = options.Value;
            _wsService = wsService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MarketPriceDbContext>();

            var assetsToSubscribe = await dbContext.Assets
                .Where(a => a.Mappings.Simulation != null && a.Mappings.Simulation.IsPresent)
                .Select(a => new
                {
                    a.Id,
                    Provider = "simulation"
                })
                .ToListAsync(stoppingToken);

            var subscriptions = assetsToSubscribe.Select(x => (x.Id, x.Provider));

            await _wsService.StartAsync(subscriptions);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }

}
