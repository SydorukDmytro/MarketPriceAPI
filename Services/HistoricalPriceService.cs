using MarketPriceAPI.Authentication;
using MarketPriceAPI.Configuration;
using MarketPriceAPI.Data;
using MarketPriceAPI.DTO;
using MarketPriceAPI.Models;
using MarketPriceAPI.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MarketPriceAPI.Services
{
    public class HistoricalPriceService : IHistoricalPriceService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MarketPriceDbContext _dbContext;
        private readonly AuthService _authService;
        private readonly FintachartsOptions _fintachartsOptions;

        public HistoricalPriceService(IHttpClientFactory httpClientFactory, MarketPriceDbContext dbContext, AuthService authService, IOptions<FintachartsOptions> fintachartsOptions)
        {
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;
            _authService = authService;
            _fintachartsOptions = fintachartsOptions.Value;
        }
        public async Task<IEnumerable<HistoricalPrice>> GetLatestBarsAsync(
            Guid instrumentId, 
            string? provider = null, 
            int? interval = null, 
            string? periodicity = null, 
            int barsCount = 10)
        {
            var token = await _authService.GetValidTokenAsync();
            var client = _httpClientFactory.CreateClient("InstrumentsClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            var queryParams = new List<string> { $"instrumentId={instrumentId}", $"barsCount={barsCount}" };

            if (!string.IsNullOrEmpty(provider))
                queryParams.Add($"provider={provider}");

            if (interval.HasValue)
                queryParams.Add($"interval={interval.Value}");

            if (!string.IsNullOrEmpty(periodicity))
                queryParams.Add($"periodicity={periodicity}");

            var url = $"/api/bars/v1/bars/count-back";
            if (queryParams.Any())
            {
                url += "?" + string.Join("&", queryParams);
            }

            Console.WriteLine($"URL: {url}");
            Console.WriteLine($"TOKEN: {token.AccessToken.Substring(0, 20)}...");
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            
            var wrapper = await JsonSerializer.DeserializeAsync<HistoricalPriceResponse>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return wrapper?.Data ?? new List<HistoricalPrice>();

        }

        public async Task<IEnumerable<HistoricalPrice>> GetBarsByDateRangeAsync(
            Guid instrumentId,
            DateTime startDate,
            DateTime? endDate = null,
            string? provider = null,
            int? interval = null,
            string? periodicity = null)
        {
            var token = await _authService.GetValidTokenAsync();
            var client = _httpClientFactory.CreateClient("InstrumentsClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            var queryParams = new List<string> { $"instrumentId={instrumentId}", $"startDate={startDate:yyyy-MM-dd}"};
            if(endDate.HasValue)
                queryParams.Add($"endDate={endDate.Value:yyyy-MM-dd}");
            if (!string.IsNullOrEmpty(provider))
                queryParams.Add($"provider={provider}");
            if (interval.HasValue)
                queryParams.Add($"interval={interval.Value}");
            if (!string.IsNullOrEmpty(periodicity))
                queryParams.Add($"periodicity={periodicity}");

            var url = $"/api/bars/v1/bars/date-range";

            if (queryParams.Any())
            {
                url += "?" + string.Join("&", queryParams);
            }

            Console.WriteLine($"URL: {url}");
            Console.WriteLine($"TOKEN: {token.AccessToken.Substring(0, 20)}...");
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            var wrapper = await JsonSerializer.DeserializeAsync<HistoricalPriceResponse>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return wrapper?.Data ?? new List<HistoricalPrice>();
        }

        public async Task<IEnumerable<HistoricalPrice>> GetBarsByTimeBackAsync(
            Guid instrumentId,
            TimeSpan timeBack,
            string? provider = null,
            int? interval = null,
            string? periodicity = null
            ){
            var token = await _authService.GetValidTokenAsync();
            var client = _httpClientFactory.CreateClient("InstrumentsClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            var queryParams = new List<string> { $"instrumentId={instrumentId}", $"timeBack={timeBack.ToString(@"d\.hh\:mm\:ss")}" };
            if (!string.IsNullOrEmpty(provider))
                queryParams.Add($"provider={provider}");
            if (interval.HasValue)
                queryParams.Add($"interval={interval.Value}");
            if (!string.IsNullOrEmpty(periodicity))
                queryParams.Add($"periodicity={periodicity}");

            var url = $"/api/bars/v1/bars/time-back";

            if (queryParams.Any())
            {
                url += "?" + string.Join("&", queryParams);
            }

            Console.WriteLine($"URL: {url}");
            Console.WriteLine($"TOKEN: {token.AccessToken.Substring(0, 20)}...");

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();

            var wrapper = await JsonSerializer.DeserializeAsync<HistoricalPriceResponse>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return wrapper?.Data ?? new List<HistoricalPrice>();
        }
    }
}
