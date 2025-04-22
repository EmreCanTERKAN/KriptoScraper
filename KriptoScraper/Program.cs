using KriptoScraper.Application.Interfaces;
using KriptoScraper.Application.Services;
using KriptoScraper.Domain.Entities;
using KriptoScraper.Domain.Interfaces;
using KriptoScraper.Domain.Services;
using KriptoScraper.Infrastructure.Services;
using KriptoScraper.Interfaces.DataStorage;
using KriptoScraper.Services.DataStorage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Config
        string tradeLogFilePath = "trades.csv";
        string summaryLogFilePath = "minute_summary.csv";

        // Domain & Application Services
        services.AddSingleton<ITradeBuffer, TradeBuffer>();
        services.AddSingleton<ITradeEventHandler, TradeEventHandler>();
        services.AddSingleton<ITradeAggregator<MinuteSummary>, MinuteTradeAggregator>(); // uygulamaya özel
        services.AddSingleton<ISummaryWriter<MinuteSummary>>(new CsvSummaryWriter<MinuteSummary>(summaryLogFilePath));
        services.AddSingleton<ITradeAggregatorService, TradeAggregatorService<MinuteSummary>>();

        // Infrastructure
        services.AddSingleton<ITradeEventWriter>(new CsvTradeEventWriter(tradeLogFilePath));
        services.AddSingleton<IBinanceWebSocketClient, BinanceWebSocketClient>();

        // App-specific service
        services.AddSingleton<TradeLoggerService>();
    })
    .Build();

// Resolve logger and start the app
var loggerService = host.Services.GetRequiredService<TradeLoggerService>();
await loggerService.StartLoggingAsync("BTCUSDT");

// Keep the app alive
Console.ReadLine();
