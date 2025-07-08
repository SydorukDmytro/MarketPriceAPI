using MarketPriceAPI.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace MarketPriceAPI.Authentication
{
    public class AuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly FintachartsOptions _fintachartsOptions;
        private Token token;
        private DateTime _expiresAt;

        public AuthService(IHttpClientFactory httpClientFactory, IOptions<FintachartsOptions> fintachartsOptions)
        {
            _httpClientFactory = httpClientFactory;
            _fintachartsOptions = fintachartsOptions.Value;
        }

        private async Task<Token> GetTokenAsync()
        {
            var client = _httpClientFactory.CreateClient("AuthClient");
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", _fintachartsOptions.Username),
                new KeyValuePair<string, string>("password", _fintachartsOptions.Password),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", _fintachartsOptions.ClientId),
                new KeyValuePair<string, string>("realm", _fintachartsOptions.Realm)
            });

            var response = await client.PostAsync($"/identity/realms/{_fintachartsOptions.Realm}/protocol/openid-connect/token", content);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var token = await JsonSerializer.DeserializeAsync<Token>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return token;
        }

        public async Task<Token> GetValidTokenAsync()
        {
            if(token == null || _expiresAt <= DateTime.UtcNow)
            {
                token = await GetTokenAsync();
                _expiresAt = DateTime.UtcNow.AddSeconds(token.ExpiresIn - 60);
            }
            Console.WriteLine($"Access token: {token?.AccessToken}");
            return token;
        }
    }
}
