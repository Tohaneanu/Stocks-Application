using Stocks.Core.DTO;

namespace Stocks.Core.ServiceContracts.StocksService
{
    public interface ISellOrderService
    {
        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);

        Task<List<SellOrderResponse>> GetSellOrders();
    }
}
