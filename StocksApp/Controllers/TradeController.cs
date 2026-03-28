using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTO;
using StocksApp.UI;
using StocksApp.UI.Models;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IFinnhubService _finnhubService;
        private readonly IStocksService _stocksService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TradeController> _logger;

        public TradeController(IOptions<TradingOptions> tradingOptions, IFinnhubService finnhubService, IConfiguration configuration, IStocksService stocksService, ILogger<TradeController> logger)
        {
            _tradingOptions = tradingOptions.Value;
            _finnhubService = finnhubService;
            _configuration = configuration;
            _stocksService = stocksService;
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
            Dictionary<string, object>? companyProfileDictionary = await _finnhubService.GetCompanyProfile(stockSymbol);

            //get stock price quotes from API server
            Dictionary<string, object>? stockQuoteDictionary = await _finnhubService.GetStockPriceQuote(stockSymbol);


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
            List<BuyOrderResponse> buyOrderResponses = await _stocksService.GetBuyOrders();
            List<SellOrderResponse> sellOrderResponses = await _stocksService.GetSellOrders();
            ViewBag.TradingOptions = _tradingOptions;
            return View(new Orders { BuyOrders = buyOrderResponses, SellOrders = sellOrderResponses });
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
        {
            //update date of order
            sellOrderRequest.DateTimeOfOrder = DateTime.Now;

            //re-validate the model object after updating the date
            ModelState.Clear();
            TryValidateModel(sellOrderRequest);

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade() { StockName = sellOrderRequest.StockName, Quantity = sellOrderRequest.Quantity, StockSymbol = sellOrderRequest.StockSymbol };
                return View("Index", stockTrade);
            }

            //invoke service method
            SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);

            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            //update date of order
            buyOrderRequest.DateTimeOfOrder = DateTime.Now;

            //re-validate the model object after updating the date
            ModelState.Clear();
            TryValidateModel(buyOrderRequest);


            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade() { StockName = buyOrderRequest.StockName, Quantity = buyOrderRequest.Quantity, StockSymbol = buyOrderRequest.StockSymbol };
                return View("Index", stockTrade);
            }

            //invoke service method
            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);

            return RedirectToAction(nameof(Orders));
        }

        [Route("OrdersPDF")]
        [HttpGet]
        public async Task<IActionResult> OrdersPDF()
        {
            //Get list of orders
            List<IOrderResponse> orders = [.. await _stocksService.GetBuyOrders(), .. await _stocksService.GetSellOrders()];
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
