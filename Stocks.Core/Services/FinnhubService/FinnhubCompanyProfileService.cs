using Stocks.Core.RepositoryContracts;
using Stocks.Core.Exceptions;
using Stocks.Core.ServiceContracts.FinnhubService;

namespace Stocks.Core.Services.FinnhubService
{
    public class FinnhubCompanyProfileService : IFinnhubCompanyProfileService
    {
        private readonly IFinnhubRepository _finnhubRepository;

        public FinnhubCompanyProfileService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            try
            {
                Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetCompanyProfile(stockSymbol);
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
