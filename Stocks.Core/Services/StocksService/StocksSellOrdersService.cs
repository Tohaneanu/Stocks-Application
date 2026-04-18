using Stocks.Core.Helpers;
using Stocks.Core.RepositoryContracts;
using Stocks.Core.Entities;
using Stocks.Core.DTO;
using Stocks.Core.ServiceContracts.StocksService;

namespace Stocks.Core.Services.StocksService
{
    public class StocksSellOrdersService: ISellOrderService
    {
        private readonly IStocksRepository _stocksRepository;
        public StocksSellOrdersService(IStocksRepository stocksRepository)
        {
            _stocksRepository = stocksRepository;
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            //Validation: sellOrderRequest can't be null
            if (sellOrderRequest == null)
                throw new ArgumentNullException(nameof(sellOrderRequest));

            //Model validation
            ValidationHelper.ModelValidation(sellOrderRequest);

            //convert sellOrderRequest into SellOrder type
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            //generate SellOrderID
            sellOrder.SellOrderId = Guid.NewGuid();

            //add sell order object to sell orders list
            SellOrder SellOrderFromRepo = await _stocksRepository.CreateSellOrder(sellOrder);
            //convert the SellOrder object into SellOrderResponse type
            return sellOrder.ToSellOrderResponse();
        }
        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            //Convert all SellOrder objects into SellOrderResponse objects
            List<SellOrder> sellOrders = await _stocksRepository.GetSellOrders();
            return sellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();
        }
    }
}
