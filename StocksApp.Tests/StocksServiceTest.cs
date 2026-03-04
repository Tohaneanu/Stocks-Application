using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTO;
using StocksApp.Services;

namespace StocksApp.Tests
{
    public class StocksServiceTest
    {
        public readonly IStocksService _stocksService;
        
        public StocksServiceTest()
        {

            _stocksService = new StocksService();
        }

        #region CreateBuyOrder
        
        [Fact]
        //When you supply BuyOrderRequest as null, it should throw ArgumentNullException
        public void CreateBuyOrder_ToBeArgumentNullException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = null;

            //Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        //When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [InlineData(0)]
        //When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [InlineData(100001)]
        public void CreateBuyOrder_InvalidQuantity_ShouldThrowArgumentException(uint buyOrderQuantity)
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = buyOrderQuantity };

            //Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Theory]
        //When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [InlineData(0)]
        //When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [InlineData(10001)]
        public void CreateBuyOrder_InvalidPrice_ShouldThrowArgumentException(uint buyOrderPrice)
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = buyOrderPrice, Quantity = 1 };

            //Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        public void CreateBuyOrder_StockSymbolIsNull_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest() { StockSymbol = null, StockName = "Microsoft", Price = 1, Quantity = 1 };

            //Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000 - 01 - 01), it should throw ArgumentException.
        public void CreateBuyOrder_DateOfOrderIsLessThanYear2000_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", DateTimeOfOrder = Convert.ToDateTime("1999-12-31"), Price = 1, Quantity = 1 };

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        //If you supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID (guid).
        public void CreateBuyOrder_ValidData_ToBeSuccessful()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", DateTimeOfOrder = Convert.ToDateTime("2024-12-31"), Price = 1, Quantity = 1 };

            //Act
            BuyOrderResponse buyOrderResponseFromCreate = _stocksService.CreateBuyOrder(buyOrderRequest);

            //Assert
            Assert.NotEqual(Guid.Empty, buyOrderResponseFromCreate.BuyOrderId);
        }

        #endregion

        #region CreateSellOrder

        [Fact]
        // When you supply SellOrderRequest as null, it should throw ArgumentNullException
        public void CreateSellOrder_NullSellOrder_ToBeArgumentNullException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = null;

            //Act
            Assert.Throws<ArgumentNullException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Theory]
        //When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [InlineData(0)]
        //When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [InlineData(100001)]
        public void CreateSellOrder_InvalidQuantity_ToBeArgumentException(uint sellOrderQuantity)
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = sellOrderQuantity };

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Theory]
        //When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [InlineData(0)]
        //When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [InlineData(10001)]
        public void CreateSellOrder_InvalidPrice_ToBeArgumentException(uint sellOrderPrice)
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = sellOrderPrice, Quantity = 1 };

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        public void CreateSellOrder_StockSymbolIsNull_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest() {StockName="TEST", StockSymbol = null, Price = 1, Quantity = 1 };

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        public void CreateSellOrder_DateOfOrderIsLessThanYear2000_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", DateTimeOfOrder = Convert.ToDateTime("1999-12-31"), Price = 1, Quantity = 1 };

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        //If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID (guid).
        public void CreateSellOrder_ValidData_ToBeSuccessful()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", DateTimeOfOrder = Convert.ToDateTime("2024-12-31"), Price = 1, Quantity = 1 };

            //Act
            SellOrderResponse sellOrderResponseFromCreate = _stocksService.CreateSellOrder(sellOrderRequest);

            //Assert
            Assert.NotEqual(Guid.Empty, sellOrderResponseFromCreate.SellOrderId);
        }


        #endregion

        #region GetBuyOrders

        [Fact]
        //When you invoke this method, by default, the returned list should be empty.
        public void GetAllBuyOrders_DefaultList_ToBeEmpty()
        {
            //Act
            List<BuyOrderResponse> buyOrdersFromGet = _stocksService.GetBuyOrders();

            //Assert
            Assert.Empty(buyOrdersFromGet);
        }

        [Fact]
        //When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
        public void GetAllBuyOrders_WithFewBuyOrders_ToBeSuccessful()
        {
            //Arrange

            //Create a list of buy orders with hard-coded data
            BuyOrderRequest buyOrder_request_1 = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = 1, DateTimeOfOrder = DateTime.Parse("2023-01-01 9:00") };

            BuyOrderRequest buyOrder_request_2 = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = 1, DateTimeOfOrder = DateTime.Parse("2023-01-01 9:00") };

            List<BuyOrderRequest> buyOrder_requests = new List<BuyOrderRequest>() { buyOrder_request_1, buyOrder_request_2 };

            List<BuyOrderResponse> buyOrder_response_list_from_add = new List<BuyOrderResponse>();

            foreach (BuyOrderRequest buyOrder_request in buyOrder_requests)
            {
                BuyOrderResponse buyOrder_response = _stocksService.CreateBuyOrder(buyOrder_request);
                buyOrder_response_list_from_add.Add(buyOrder_response);
            }

            //Act
            List<BuyOrderResponse> buyOrders_list_from_get = _stocksService.GetBuyOrders();

            //Assert
            foreach (BuyOrderResponse buyOrder_response_from_add in buyOrder_response_list_from_add)
            {
                Assert.Contains(buyOrder_response_from_add, buyOrders_list_from_get);
            }
        }

        #endregion

        #region GetSellOrders

        [Fact]
        //When you invoke this method, by default, the returned list should be empty.
        public void GetAllSellOrders_DefaultList_ToBeEmpty()
        {
            //Act
            List<SellOrderResponse> sellOrdersFromGet = _stocksService.GetSellOrders();

            //Assert
            Assert.Empty(sellOrdersFromGet);
        }


        [Fact]
        //When you first add few sell orders using CreateSellOrder() method; and then invoke GetAllSellOrders() method; the returned list should contain all the same sell orders.
        public void GetAllSellOrders_WithFewSellOrders_ToBeSuccessful()
        {
            //Arrange

            //Create a list of sell orders with hard-coded data
            SellOrderRequest sellOrder_request_1 = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = 1, DateTimeOfOrder = DateTime.Parse("2023-01-01 9:00") };

            SellOrderRequest sellOrder_request_2 = new SellOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = 1, DateTimeOfOrder = DateTime.Parse("2023-01-01 9:00") };

            List<SellOrderRequest> sellOrder_requests = new List<SellOrderRequest>() { sellOrder_request_1, sellOrder_request_2 };

            List<SellOrderResponse> sellOrder_response_list_from_add = new List<SellOrderResponse>();

            foreach (SellOrderRequest sellOrder_request in sellOrder_requests)
            {
                SellOrderResponse sellOrder_response = _stocksService.CreateSellOrder(sellOrder_request);
                sellOrder_response_list_from_add.Add(sellOrder_response);
            }

            //Act
            List<SellOrderResponse> sellOrders_list_from_get = _stocksService.GetSellOrders();

            //Assert
            foreach (SellOrderResponse sellOrder_response_from_add in sellOrder_response_list_from_add)
            {
                Assert.Contains(sellOrder_response_from_add, sellOrders_list_from_get);
            }
        }

        #endregion

    }
}