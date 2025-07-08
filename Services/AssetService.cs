using MarketPriceAPI.Authentication;
using MarketPriceAPI.Configuration;
using MarketPriceAPI.Data;
using MarketPriceAPI.DTO;
using MarketPriceAPI.Models;
using MarketPriceAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MarketPriceAPI.Services
{
    public class AssetService : IAssetService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MarketPriceDbContext _dbContext;
        private readonly AuthService _authService;
        private readonly FintachartsOptions _fintachartsOptions;

        public AssetService(IHttpClientFactory httpClientFactory, MarketPriceDbContext dbContext, AuthService authService, IOptions<FintachartsOptions> fintachartsOptions)
        {
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;
            _authService = authService;
            _fintachartsOptions = fintachartsOptions.Value;
        }

        public async Task<IEnumerable<Asset>> GetAllAssetsAsync(string? provider = null, string? kind = null)
        {
            var localAssets = string.IsNullOrEmpty(kind)
                ? await _dbContext.Assets.ToListAsync()
                : await _dbContext.Assets.Where(a => a.Kind == kind).ToListAsync();

            if (localAssets.Any())
            {
                return localAssets;
            }
            // If no local assets, fetch from external API
            var token = await _authService.GetValidTokenAsync();
            var client = _httpClientFactory.CreateClient("InstrumentsClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(provider))
            {
                queryParams.Add($"provider={provider}");
            }
            if (!string.IsNullOrEmpty(kind))
            {
                queryParams.Add($"kind={kind}");
            }
            var url = "/api/instruments/v1/instruments";
            if (queryParams.Any())
            {
                url += "?" + string.Join("&", queryParams);
            }
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var wrapper = await JsonSerializer.DeserializeAsync<InstrumentListResponse>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var fetchedAssets = wrapper?.Data ?? new List<Asset>();

            foreach(var asset in fetchedAssets)
            {
                // Check if the asset already exists in the database
                if (!await _dbContext.Assets.AnyAsync(a => a.Id == asset.Id))
                {
                    // Add new asset to the database
                    _dbContext.Assets.Add(asset);
                }
            }

            await _dbContext.SaveChangesAsync();

            return fetchedAssets;
        }

        public Task<Asset> GetAssetBySymbolAsync()
        {
            throw new NotImplementedException();
        }
    }
}
