using KriptoScraper.Application.Aggregators;
using KriptoScraper.Application.Buffers;
using KriptoScraper.Application.Entities;
using KriptoScraper.Application.Handlers;
using KriptoScraper.Application.Helpers;
using KriptoScraper.Application.Interfaces;
using KriptoScraper.Application.Services;
using KriptoScraper.Application.Settings;
using KriptoScraper.Domain.Interfaces;
using KriptoScraper.Infrastructure.WebSocketClients;
using KriptoScraper.Infrastructure.Writers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KriptoScraper.Infrastructure;
public static class InfrastructureRegistrar
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TradeSettings>(configuration.GetSection("TradeSettings"));
        services.AddSingleton<IBinanceWebSocketClient, BinanceWebSocketClient>();
        services.AddSingleton<ITradeEventHandler, TradeEventHandler>();
        services.AddSingleton<ITradeLoggerService, TradeLoggerService>();
        services.AddSingleton<ITradeBuffer, TradeBuffer>();
        services.AddSingleton<ITradeEventWriter, CsvTradeEventWriter>();
        services.AddSingleton<ILogFilePathProvider, DefaultLogFilePathProvider>();
        services.AddSingleton<ITradeAggregator<MinuteSummary>, MinuteTradeAggregator>();

        var settings = configuration
            .GetSection("TradeSettings")
            .Get<TradeSettings>();
        foreach (var pair in settings!.Pairs)
        {
            //ISummaryWriter<MinuteSummary>
            services.AddSingleton<ISummaryWriter<MinuteSummary>>(sp =>
            {
                var pathProvider = sp.GetRequiredService<ILogFilePathProvider>();
                return new CsvSummaryWriter<MinuteSummary>(
                    pathProvider,
                    pair.Symbol,
                    pair.Timeframe.ToTimeSpan()
                    );
            });
            //ITradeAggregatorService
            services.AddSingleton<ITradeAggregatorService>(sp =>
            {
                var buffer = sp.GetRequiredService<ITradeBuffer>();
                var aggregator = sp.GetRequiredService<ITradeAggregator<MinuteSummary>>();
                var writer = sp.GetRequiredService<ISummaryWriter<MinuteSummary>>();
                var interval = pair.Timeframe.ToTimeSpan();

                return new TradeAggregatorService<MinuteSummary>(
                    buffer,
                    aggregator,
                    writer,
                    interval);
            });
        }

        return services;
    }
}
