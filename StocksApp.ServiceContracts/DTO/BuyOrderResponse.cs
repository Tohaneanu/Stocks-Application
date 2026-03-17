using StocksApp.Entities;

namespace StocksApp.ServiceContracts.DTO
{
    public class BuyOrderResponse : IOrderResponse
    {
        public Guid BuyOrderId { get; set; } 
        public required string StockSymbol { get; set; }
        public required string StockName { get; set; }
        public DateTime DateTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public OrderType TypeOfOrder => OrderType.BuyOrder;
        public double TradeAmount { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is not BuyOrderResponse) return false;

            BuyOrderResponse other = (BuyOrderResponse)obj;
            return BuyOrderId == other.BuyOrderId && StockSymbol == other.StockSymbol && StockName == other.StockName && DateTimeOfOrder == other.DateTimeOfOrder && Quantity == other.Quantity && Price == other.Price;
        }

        public override int GetHashCode()
        {
            return StockSymbol.GetHashCode();
        }
        public override string ToString()
        {
            return $"Buy Order ID: {BuyOrderId}, Stock Symbol: {StockSymbol}, Stock Name: {StockName}, Date and Time of Buy Order: {DateTimeOfOrder.ToString("dd MMM yyyy hh:mm ss tt")}, Quantity: {Quantity}, Buy Price: {Price}, Trade Amount: {TradeAmount}";
        }
    
}
    public static class BuyOrderExtensions
    {
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse() { BuyOrderId = buyOrder.BuyOrderId, StockSymbol = buyOrder.StockSymbol, StockName = buyOrder.StockName, Price = buyOrder.Price, DateTimeOfOrder = buyOrder.DateTimeOfOrder, Quantity = buyOrder.Quantity, TradeAmount = buyOrder.Price * buyOrder.Quantity };
        }
    }
}
