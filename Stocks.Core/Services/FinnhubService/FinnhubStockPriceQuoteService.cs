using Stocks.Core.RepositoryContracts;
using Stocks.Core.Exceptions;
using Stocks.Core.ServiceContracts.FinnhubService;

namespace Stocks.Core.Services.FinnhubService
{
    public class FinnhubStockPriceQuoteService : IFinnhubStockPriceQuoteService
    {
        private readonly IFinnhubRepository _finnhubRepository;

        public FinnhubStockPriceQuoteService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            try
            {
                Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetStockPriceQuote(stockSymbol);
                return responseDictionary;
            }
            catch (Exception ex)
            {
                FinnhubException finnhubException = new FinnhubException("Unable to connect to finnhub", ex);
                throw finnhubException;
            }
        }
    }
}
