using KriptoScraper.Application.Interfaces;
using KriptoScraper.Application.Services;
using KriptoScraper.Domain.Entities;
using KriptoScraper.Domain.Interfaces;
using KriptoScraper.Domain.Services;
using KriptoScraper.Infrastructure.Services;
using KriptoScraper.Interfaces.DataStorage;
using KriptoScraper.Services.DataStorage;
using KriptoScraper.Worker;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;

        // Configuration
        string symbol = config.GetValue<string>("TradeSettings:Symbol")!;
        const string TradeLogFileName = "trades.csv";
        const string SummaryLogFileName = "minute_summary.csv";
        var aggregationInterval = TimeSpan.FromMinutes(1);

        // Domain Layer
        services.AddSingleton<ITradeBuffer, TradeBuffer>();
        services.AddSingleton<ITradeAggregator<MinuteSummary>, MinuteTradeAggregator>();

        // Infrastructure Layer
        services.AddSingleton<ITradeEventWriter>(new CsvTradeEventWriter(TradeLogFileName));
        services.AddSingleton<ISummaryWriter<MinuteSummary>>(new CsvSummaryWriter<MinuteSummary>(SummaryLogFileName));
        services.AddSingleton<IBinanceWebSocketClient, BinanceWebSocketClient>();

        // Application Layer
        services.AddSingleton<ITradeAggregatorService>(provider =>
            new TradeAggregatorService<MinuteSummary>(
                provider.GetRequiredService<ITradeBuffer>(),
                provider.GetRequiredService<ITradeAggregator<MinuteSummary>>(),
                provider.GetRequiredService<ISummaryWriter<MinuteSummary>>(),
                aggregationInterval
            ));
        services.AddSingleton<ITradeEventHandler, TradeEventHandler>();
        services.AddSingleton<TradeLoggerService>();

        // UI Layer (Worker)
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();

