using StocksApp.Entities;

namespace StocksApp.ServiceContracts.DTO
{
    public class SellOrderResponse: IOrderResponse
    {
        public Guid SellOrderId { get; set; }
        public required string StockSymbol { get; set; }
        public required string StockName { get; set; }
        public DateTime DateTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public OrderType TypeOfOrder => OrderType.SellOrder;
        public double TradeAmount { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is not SellOrderResponse) return false;

            SellOrderResponse other = (SellOrderResponse)obj;
            return SellOrderId == other.SellOrderId && StockSymbol == other.StockSymbol && StockName == other.StockName && DateTimeOfOrder == other.DateTimeOfOrder && Quantity == other.Quantity && Price == other.Price;
        }

        public override int GetHashCode()
        {
            return StockSymbol.GetHashCode();
        }

        public override string ToString()
        {
            return $"Sell Order ID: {SellOrderId}, Stock Symbol: {StockSymbol}, Stock Name: {StockName}, Date and Time of Sell Order: {DateTimeOfOrder.ToString("dd MMM yyyy hh:mm ss tt")}, Quantity: {Quantity}, Sell Price: {Price}, Trade Amount: {TradeAmount}";
        }
    }

    public static class SellOrderExtensions
    {
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse() { SellOrderId = sellOrder.SellOrderId, StockSymbol = sellOrder.StockSymbol, StockName = sellOrder.StockName, Price = sellOrder.Price, DateTimeOfOrder = sellOrder.DateTimeOfOrder, Quantity = sellOrder.Quantity, TradeAmount = sellOrder.Price * sellOrder.Quantity };
        }
    }
}
