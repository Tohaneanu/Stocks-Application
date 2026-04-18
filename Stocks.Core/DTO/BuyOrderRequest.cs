using Stocks.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Stocks.Core.DTO
{
    public class BuyOrderRequest : IValidatableObject, IOrderRequest
    {
        [Required(ErrorMessage = "Stock Symbol can't be null or empty")]
        public required string StockSymbol { get; set; }
        [Required(ErrorMessage = "Stock Name can't be null or empty")]
        public required string StockName { get; set; }
        public DateTime DateTimeOfOrder { get; set; }
        [Range(1, 100000, ErrorMessage = "You can buy maximum of 100000 shares in single order. Minimum is 1.")]
        public uint Quantity { get; set; }
        [Range(1, 10000, ErrorMessage = "The maximum price of stock is 10000. Minimum is 1.")]
        public double Price { get; set; }

        public BuyOrder ToBuyOrder()
        {
            return new BuyOrder() { StockSymbol = StockSymbol, StockName = StockName, Price = Price, DateTimeOfOrder = DateTimeOfOrder, Quantity = Quantity };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            //Date of order should be less than Jan 01, 2000
            if (DateTimeOfOrder < Convert.ToDateTime("2000-01-01"))
            {
                results.Add(new ValidationResult("Date of the order should not be older than Jan 01, 2000."));
            }

            return results;
        }
    }
}
