using Stocks.Core.DTO;

namespace Stocks.Core.ServiceContracts.StocksService
{
    public interface IBuyOrderService
    {
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);
        Task<List<BuyOrderResponse>> GetBuyOrders();
    }
}
