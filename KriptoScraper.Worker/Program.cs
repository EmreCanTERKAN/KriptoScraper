using KriptoScraper.Application.Interfaces;
using KriptoScraper.Application.Services;
using KriptoScraper.Domain.Entities;
using KriptoScraper.Domain.Interfaces;
using KriptoScraper.Domain.Services;
using KriptoScraper.Infrastructure.Interfaces;
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
        string TradeLogFileName = $"logs/{symbol}/{symbol}_trades.csv";
        string SummaryLogFileName = $"{symbol}/{symbol}_minute_summary.csv";
        var aggregationInterval = TimeSpan.FromMinutes(1);

        // Domain Layer
        services.AddSingleton<ITradeBuffer, TradeBuffer>();
        services.AddSingleton<ITradeAggregator<MinuteSummary>, MinuteTradeAggregator>();
        services.AddSingleton<ISymbolProvider, AppSettingsSymbolProvider>();

        // Infrastructure Layer
        services.AddSingleton<ITradeEventWriter>(new CsvTradeEventWriter(TradeLogFileName));
        services.AddSingleton<ISummaryWriter<MinuteSummary>>(provider =>
            new CsvSummaryWriter<MinuteSummary>(
                provider.GetRequiredService<ILogFilePathProvider>(),
                symbol: config["TradeSettings:Symbol"]!,
                interval: "1m"
            ));

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
        services.AddSingleton<ISummaryFilePathProvider, SummaryFilePathProvider>();

        // UI Layer (Worker)
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();

