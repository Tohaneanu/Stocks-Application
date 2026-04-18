namespace Stocks.Core.DTO
{
    public interface IOrderRequest
    {
        public string StockSymbol { get; set; }
        public string StockName { get; set; }
        public DateTime DateTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
    }
}
