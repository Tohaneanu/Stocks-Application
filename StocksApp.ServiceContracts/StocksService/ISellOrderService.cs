using StocksApp.ServiceContracts.DTO;

namespace StocksApp.ServiceContracts.StocksService
{
    public interface ISellOrderService
    {
        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);

        Task<List<SellOrderResponse>> GetSellOrders();
    }
}
