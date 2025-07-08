using MarketPriceAPI.Authentication;
using MarketPriceAPI.Configuration;
using MarketPriceAPI.Data;
using MarketPriceAPI.Services;
using MarketPriceAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("https://+:8080");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<WebSocketRealTimePriceService>();
builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddScoped<IHistoricalPriceService, HistoricalPriceService>();
builder.Services.AddScoped<IRealTimePriceService, RealTimePriceService>();
builder.Services.AddHostedService<RealtimePriceBackgroundService>();

var configuration = builder.Configuration;
var provider = configuration.GetValue<string>("DatabaseProvider");

builder.Services.AddDbContext<MarketPriceDbContext>(options =>
{
    if (provider == "SqlServer")
    {
        options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
    }
    else if (provider == "SQLite")
    {
        options.UseSqlite(configuration.GetConnectionString("SQLite"));
    }
    else
    {
        throw new InvalidOperationException("Unsupported database provider");
    }
});

builder.Services.Configure<FintachartsOptions>(builder.Configuration.GetSection("Fintacharts"));
var fintachartsOptions = builder.Configuration.GetSection("Fintacharts").Get<FintachartsOptions>();
builder.Services.Configure<RealtimeSubscriptionOptions>(
    builder.Configuration.GetSection("RealtimeSubscription"));

builder.Services.AddHttpClient("AuthClient", client =>
{
    client.BaseAddress = new Uri(fintachartsOptions.BaseUrl);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

});

builder.Services.AddHttpClient("InstrumentsClient", client =>
{
    client.BaseAddress = new Uri(fintachartsOptions.BaseUrl);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
