using Services.Helpers;
using StockApp.RepositoryContracts;
using StocksApp.Entities;
using StocksApp.ServiceContracts.DTO;
using StocksApp.ServiceContracts.StocksService;

namespace StocksApp.Services.StocksService
{
    public class StocksBuyOrdersService: IBuyOrderService
    {
        private readonly IStocksRepository _stocksRepository;
        public StocksBuyOrdersService(IStocksRepository stocksRepository)
        {
            _stocksRepository = stocksRepository;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
                throw new ArgumentNullException(nameof(buyOrderRequest));

            //Model validation
            ValidationHelper.ModelValidation(buyOrderRequest);

            //convert buyOrderRequest into BuyOrder type
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            //generate BuyOrderID
            buyOrder.BuyOrderId = Guid.NewGuid();

            //add buy order object to buy orders list
            BuyOrder buyOrderFromRepo = await _stocksRepository.CreateBuyOrder(buyOrder);
            //convert the BuyOrder object into BuyOrderResponse type
            return buyOrder.ToBuyOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            //Convert all BuyOrder objects into BuyOrderResponse objects
            List<BuyOrder> buyOrders = await _stocksRepository.GetBuyOrders();
            return buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();
        }
    }
}
