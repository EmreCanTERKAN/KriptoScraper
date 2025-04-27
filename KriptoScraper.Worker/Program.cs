using KriptoScraper.Application.Aggregators;
using KriptoScraper.Domain.Entities;
using KriptoScraper.Domain.Handlers;
using KriptoScraper.Domain.Interfaces;
using KriptoScraper.Domain.Services;
using KriptoScraper.Infrastructure;
using KriptoScraper.Worker;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx,services) =>
    {
        services.AddInfrastructure(ctx.Configuration);
        services.AddHostedService<Worker>();
        services.AddSingleton<ITradeLoggerService, TradeLoggerService>();
        services.AddSingleton<ITradeEventHandler, TradeEventHandler>();
        services.AddSingleton<ITradeAggregatorService, TradeAggregatorService<MinuteSummary>>();
        services.AddScoped<ITradeAggregator<MinuteSummary>, MinuteTradeAggregator>();

    })
    .Build();

await host.RunAsync();

