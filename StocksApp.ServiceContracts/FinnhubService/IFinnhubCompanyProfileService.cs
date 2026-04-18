namespace StocksApp.ServiceContracts.FinnhubService
{
    public interface IFinnhubCompanyProfileService
    {
        /// <summary>
        /// Returns company details such as company country, currency, exchange, IPO date, logo image, market capitalization, name of the company, phone number etc.
        /// </summary>
        /// <param name="stockSymbol">Stock symbol to search</param>
        /// <returns>Returns a dictionary that contains details such as company country, currency, exchange, IPO date, logo image, market capitalization, name of the company, phone number etc.</returns>
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
    }
}
