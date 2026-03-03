using Microsoft.Extensions.Configuration;
using System.Text.Json;
using StocksApp.ServiceContracts;

namespace StocksApp.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinnhubService(IHttpClientFactory httpClientFactory,
                              IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            return SendRequestAsync($"stock/profile2?symbol={stockSymbol}");
        }

        public Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            return SendRequestAsync($"quote?symbol={stockSymbol}");
        }


        private async Task<Dictionary<string, object>?> SendRequestAsync(string endpoint)
        {
            string? token = _configuration["StockApiKey"];

            if (string.IsNullOrWhiteSpace(token))
                throw new InvalidOperationException("Finnhub token is not configured.");

            HttpClient client = _httpClientFactory.CreateClient("Finnhub");

            HttpResponseMessage response =
                await client.GetAsync($"{endpoint}&token={token}");

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(
                    $"Finnhub API error: {response.StatusCode}");

            string responseBody = await response.Content.ReadAsStringAsync();

            var responseDictionary =
                JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

            if (responseDictionary == null)
                throw new InvalidOperationException("No response from server.");

            if (responseDictionary.ContainsKey("error"))
                throw new InvalidOperationException(
                    Convert.ToString(responseDictionary["error"]));

            return responseDictionary;
        }
    }
}