using Microsoft.Extensions.Configuration;
using StockApp.RepositoryContracts;
using System.Text.Json;

namespace StockApp.Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinnhubRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        private async Task<T?> SendRequestAsync<T>(string endpoint)
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

            T? result = JsonSerializer.Deserialize<T>(responseBody);

            if (result == null)
                throw new InvalidOperationException("No response from server.");

            // verificare pentru cazurile cu "error" (doar dacă e Dictionary)
            if (result is Dictionary<string, object> dict &&
                dict.ContainsKey("error"))
            {
                throw new InvalidOperationException(
                    Convert.ToString(dict["error"]));
            }

            return result;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            return await SendRequestAsync<Dictionary<string, object>>(
                $"stock/profile2?symbol={stockSymbol}");
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            return await SendRequestAsync<Dictionary<string, object>>(
                $"quote?symbol={stockSymbol}");
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            return await SendRequestAsync<List<Dictionary<string, string>>>(
                "stock/symbol?exchange=US");
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            return await SendRequestAsync<Dictionary<string, object>>(
                $"search?q={stockSymbolToSearch}");
        }
    }
}
