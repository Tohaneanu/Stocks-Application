using Microsoft.AspNetCore.Mvc.Filters;
using StocksApp.Controllers;
using StocksApp.ServiceContracts.DTO;
using StocksApp.UI.Models;

namespace StocksApp.UI.Filters.ActionFilters
{
    public class CreateOrderActionFilter : IAsyncActionFilter
    {
        public CreateOrderActionFilter()
        {
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //TO DO: before logic

            if (context.Controller is TradeController tradeController)
            {
                var orderRequest = context.ActionArguments.Values.FirstOrDefault() as IOrderRequest;

                if (orderRequest != null)
                {

                    //update date of order
                    orderRequest.DateTimeOfOrder = DateTime.Now;

                    //re-validate the model object after updating the date
                    tradeController.ModelState.Clear();
                    tradeController.TryValidateModel(orderRequest);


                    if (!tradeController.ModelState.IsValid)
                    {
                        tradeController.ViewBag.Errors = tradeController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();



                        StockTrade stockTrade = new StockTrade() { StockName = orderRequest.StockName, Quantity = orderRequest.Quantity, StockSymbol = orderRequest.StockSymbol };

                        context.Result = tradeController.View(nameof(TradeController.Index), stockTrade); //short-circuits or skips the subsequent action filters & action method
                    }

                    else
                    {
                        await next(); //invokes the subsequent filter or action method
                    }
                }
                else
                {
                    await next(); //invokes the subsequent filter or action method
                }
            }
            else
            {
                await next(); //calls the subsequent filter or action method
            }

            //TO DO: before logic
        }
    }
}
