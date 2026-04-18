using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using StocksApp.ServiceContracts.DTO;
using StocksApp.ServiceContracts.FinnhubService;
using StocksApp.ServiceContracts.StocksService;
using StocksApp.UI;
using StocksApp.UI.Filters.ActionFilters;
using StocksApp.UI.Models;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IBuyOrderService _stocksBuyOrdersService;
        private readonly ISellOrderService _stocksSellOrdersService;
        private readonly IFinnhubCompanyProfileService _finnhubCompanyProfileService;
        private readonly IFinnhubStockPriceQuoteService _finnhubStockPriceQuoteService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TradeController> _logger;

        public TradeController(IOptions<TradingOptions> tradingOptions, IBuyOrderService stocksBuyOrdersService, ISellOrderService stocksSellOrdersService, IFinnhubCompanyProfileService finnhubCompanyProfileService, IFinnhubStockPriceQuoteService finnhubStockPriceQuoteService, IConfiguration configuration, ILogger<TradeController> logger)
        {
            _tradingOptions = tradingOptions.Value;
            _stocksBuyOrdersService = stocksBuyOrdersService;
            _stocksSellOrdersService = stocksSellOrdersService;
            _finnhubCompanyProfileService = finnhubCompanyProfileService;
            _finnhubStockPriceQuoteService = finnhubStockPriceQuoteService;
            _configuration = configuration;
            _logger = logger;
        }

        [Route("[action]/{stockSymbol?}")]
        [Route("~/[controller]/{stockSymbol?}")]
        public async Task<IActionResult> Index(string stockSymbol)
        {
            _logger.LogInformation("In TradeController.Index() action method");
            _logger.LogDebug("stockSymbol: {stockSymbol}", stockSymbol);
            //reset stock symbol if not exists
            if (string.IsNullOrEmpty(stockSymbol))
                stockSymbol = "MSFT";


            //get company profile from API server
            Dictionary<string, object>? companyProfileDictionary = await _finnhubCompanyProfileService.GetCompanyProfile(stockSymbol);

            //get stock price quotes from API server
            Dictionary<string, object>? stockQuoteDictionary = await _finnhubStockPriceQuoteService.GetStockPriceQuote(stockSymbol);


            //create model object
            StockTrade stockTrade = new StockTrade() { StockSymbol = stockSymbol };

            //load data from finnHubService into model object
            if (companyProfileDictionary != null && stockQuoteDictionary != null)
            {
                stockTrade = new StockTrade()
                {
                    StockSymbol = Convert.ToString(companyProfileDictionary["ticker"]),
                    StockName = Convert.ToString(companyProfileDictionary["name"]),
                    Quantity = _tradingOptions.DefaultOrderQuantity ?? 0,
                    Price = Convert.ToDouble(stockQuoteDictionary["c"].ToString())
                };
            }

            //Send Finnhub token to view
            ViewBag.FinnhubToken = _configuration["StockApiKey"];

            return View(stockTrade);
        }

        [Route("[action]")]
        public async Task<IActionResult> Orders()
        {
            List<BuyOrderResponse> buyOrderResponses = await _stocksBuyOrdersService.GetBuyOrders();
            List<SellOrderResponse> sellOrderResponses = await _stocksSellOrdersService.GetSellOrders();
            ViewBag.TradingOptions = _tradingOptions;
            return View(new Orders { BuyOrders = buyOrderResponses, SellOrders = sellOrderResponses });
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
        {
            //invoke service method
            SellOrderResponse sellOrderResponse = await _stocksSellOrdersService.CreateSellOrder(sellOrderRequest);

            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            //invoke service method
            BuyOrderResponse buyOrderResponse = await _stocksBuyOrdersService.CreateBuyOrder(buyOrderRequest);

            return RedirectToAction(nameof(Orders));
        }

        [Route("OrdersPDF")]
        [HttpGet]
        public async Task<IActionResult> OrdersPDF()
        {
            //Get list of orders
            List<IOrderResponse> orders = [.. await _stocksBuyOrdersService.GetBuyOrders(), .. await _stocksSellOrdersService.GetSellOrders()];
            orders = orders.OrderByDescending(temp => temp.DateTimeOfOrder).ToList();
            ViewBag.TradingOptions = _tradingOptions;

            //Return view as pdf
            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };

        }
    }
}
