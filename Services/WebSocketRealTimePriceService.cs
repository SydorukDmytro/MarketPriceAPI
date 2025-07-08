using MarketPriceAPI.Authentication;
using MarketPriceAPI.Data;
using MarketPriceAPI.DTO;
using MarketPriceAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace MarketPriceAPI.Services
{
    public class WebSocketRealTimePriceService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private ClientWebSocket _webSocket;

        public WebSocketRealTimePriceService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _webSocket = new ClientWebSocket();
        }

        public async Task StartAsync(IEnumerable<(Guid InstrumentId, string Provider)> subscriptions)
        {
            using var scope = _scopeFactory.CreateScope();
            var authService = scope.ServiceProvider.GetRequiredService<AuthService>();
            var token = await authService.GetValidTokenAsync();

            var uri = new Uri($"wss://platform.fintacharts.com/api/streaming/ws/v1/realtime?token={token.AccessToken}");
            await _webSocket.ConnectAsync(uri, CancellationToken.None);
            Console.WriteLine("✅ Connected to WebSocket");

            foreach (var (instrumentId, provider) in subscriptions)
            {
                var subscribeMessage = new
                {
                    type = "l1-subscription",
                    id = Guid.NewGuid().ToString(),
                    instrumentId = instrumentId.ToString(),
                    provider,
                    subscribe = true,
                    kinds = new[] { "ask", "bid", "last" }
                };

                var json = JsonSerializer.Serialize(subscribeMessage);
                var bytes = Encoding.UTF8.GetBytes(json);
                await _webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                Console.WriteLine($"📩 Subscribed to instrument {instrumentId} / provider {provider}");
            }

            _ = ReceiveLoopAsync();
        }

        private async Task ReceiveLoopAsync()
        {
            var buffer = new byte[4096];

            while (_webSocket.State == WebSocketState.Open)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"📨 Received: {message}");

                try
                {
                    var update = JsonSerializer.Deserialize<RealtimePriceDto>(message, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (update == null) continue;

                    using var scope = _scopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<MarketPriceDbContext>();

                    var instrumentGuid = Guid.Parse(update.InstrumentId);
                    var asset = await dbContext.Assets.FirstOrDefaultAsync(a => a.Id == instrumentGuid);
                    if (asset == null) return;

                    var snapshot = new RealTimePriceSnapshot
                    {
                        instrumentId = instrumentGuid,
                        Ask = update.Ask?.Price ?? 0m,
                        Bid = update.Bid?.Price ?? 0m,
                        Last = update.Last?.Price ?? 0m,
                        Symbol = asset.Symbol ?? string.Empty,
                        Description = asset.Description ?? string.Empty,
                        Timestamp = DateTime.UtcNow
                    };

                    dbContext.RealTimePriceSnapshots.Add(snapshot);
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Deserialization or DB error: {ex.Message}");
                }
            }
        }
    }


}
