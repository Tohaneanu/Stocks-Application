namespace StocksApp.UI
{
    /// <summary>
    /// Represents Options pattern for "StockPrice" configuration
    /// </summary>
    public class TradingOptions
    {
        public string? DefaultStockSymbol { get; set; }
        public string? DefaultOrderQuantity { get; set; }
    }
}
