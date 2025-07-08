using MarketPriceAPI.Models;

namespace MarketPriceAPI.Services.Interfaces
{
    public interface IAssetService
    {
        public Task<IEnumerable<Asset>> GetAllAssetsAsync(string? provider = null, string? kind = null);
        public Task<Asset> GetAssetBySymbolAsync();
    }
}
