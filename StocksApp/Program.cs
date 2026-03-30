using Microsoft.EntityFrameworkCore;
using Serilog;
using StockApp.Repositories;
using StockApp.RepositoryContracts;
using StocksApp.Entities;
using StocksApp.ServiceContracts;
using StocksApp.Services;
using StocksApp.UI;
using StocksApp.UI.Middleware;

var builder = WebApplication.CreateBuilder(args);

//Serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) => {

    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration) //read configuration settings from built-in IConfiguration
    .ReadFrom.Services(services); //read out current app's services and make them available to serilog
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
builder.Services.AddTransient<IFinnhubService, FinnhubService>();
builder.Services.AddTransient<IStocksService, StocksService>();
builder.Services.AddTransient<IStocksRepository, StocksRepository>();
builder.Services.AddTransient<IFinnhubRepository, FinnhubRepository>();

builder.Services.AddDbContext<StockMarketDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
});

builder.Services.AddHttpClient("Finnhub", client =>
{
    client.BaseAddress = new Uri("https://finnhub.io/api/v1/");
});
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    builder.Configuration.AddUserSecrets<Program>();
}
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
if (!builder.Environment.IsEnvironment("Test"))
{
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
}
app.MapStaticAssets();
app.MapGet("/", context =>
{
    context.Response.Redirect("/Trade/Index");
    return Task.CompletedTask;
});
app.MapControllers();

app.Run();

public partial class Program { }