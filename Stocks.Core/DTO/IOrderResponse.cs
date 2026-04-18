namespace Stocks.Core.DTO
{
    public interface IOrderResponse
    {
        string StockSymbol { get; set; }
        string StockName { get; set; }
        DateTime DateTimeOfOrder { get; set; }
        uint Quantity { get; set; }
        double Price { get; set; }
        OrderType TypeOfOrder { get; }
        double TradeAmount { get; set; }
    }

    public enum OrderType
    {
        BuyOrder, SellOrder
    }
}