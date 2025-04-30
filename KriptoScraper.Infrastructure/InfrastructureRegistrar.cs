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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
        services.AddSingleton<ISummaryReader<MinuteSummary>, CsvSummaryReader>();
        services.AddSingleton<ISummaryAggregator<MinuteSummary>, MultiMinuteAggregator>();

        // ISummaryWriter<MinuteSummary> için servis kaydını ekle
        services.AddSingleton<IEnumerable<ISummaryWriter<MinuteSummary>>>(sp =>
        {
            var pathProvider = sp.GetRequiredService<ILogFilePathProvider>();
            var tradeSettings = sp.GetRequiredService<IOptions<TradeSettings>>().Value;

            // Her sembol için CsvSummaryWriter<MinuteSummary> oluşturuyoruz
            return tradeSettings.Pairs.Select(pair =>
            {
                return new CsvSummaryWriter<MinuteSummary>(
                    pathProvider,   // pathProvider'ı alıyoruz
                    pair.Symbol,    // sembolü alıyoruz
                    pair.Timeframe.ToTimeSpan() // interval'ı alıyoruz
                );
            }).ToList();  // Tüm CsvSummaryWriter<MinuteSummary> örneklerini içeren bir liste döndürüyoruz
        });

        services.AddSingleton<IEnumerable<ISummaryWriter<MinuteSummary>>>(sp =>
        {
            var pathProvider = sp.GetRequiredService<ILogFilePathProvider>();
            var tradeSettings = sp.GetRequiredService<IOptions<TradeSettings>>().Value;

            return tradeSettings.Pairs.Select(pair =>
                new CsvSummaryWriter<MinuteSummary>(
                    pathProvider,
                    pair.Symbol,
                    pair.Timeframe.ToTimeSpan()
                )
            ).ToList();
        });

        // TradeAggregatorService kayıtlarını ekliyoruz
        services.AddSingleton<IEnumerable<ITradeAggregatorService>>(sp =>
        {
            var tradeSettings = sp.GetRequiredService<IOptions<TradeSettings>>().Value;
            var buffer = sp.GetRequiredService<ITradeBuffer>();
            var aggregator = sp.GetRequiredService<ITradeAggregator<MinuteSummary>>();
            var allWriters = sp.GetRequiredService<IEnumerable<ISummaryWriter<MinuteSummary>>>();

            return tradeSettings.Pairs.Select(pair =>
            {
                var interval = pair.Timeframe.ToTimeSpan();

                // Writer'ı sembol ve interval'e göre bul
                var writer = allWriters.First(w =>
                    (w as CsvSummaryWriter<MinuteSummary>)?.Symbol == pair.Symbol &&
                    (w as CsvSummaryWriter<MinuteSummary>)?.Interval == interval);

                return new TradeAggregatorService<MinuteSummary>(
                    buffer,
                    aggregator,
                    writer,
                    interval
                );
            }).ToList();
        });

        // Diğer servisleri kaydediyoruz
        services.AddSingleton<IEnumerable<ISummaryProcessingService>>(sp =>
        {
            var tradeSettings = sp.GetRequiredService<IOptions<TradeSettings>>().Value;
            var reader = sp.GetRequiredService<ISummaryReader<MinuteSummary>>();
            var aggregator = sp.GetRequiredService<ISummaryAggregator<MinuteSummary>>();
            var pathProvider = sp.GetRequiredService<ILogFilePathProvider>();
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            var allWriters = sp.GetRequiredService<IEnumerable<ISummaryWriter<MinuteSummary>>>();

            return tradeSettings.Pairs.Select(pair =>
            {
                var interval = pair.Timeframe.ToTimeSpan();

                // writer'ı symbol + interval eşleşmesine göre bul
                var writer = allWriters.First(w =>
                    (w as CsvSummaryWriter<MinuteSummary>)?.Symbol == pair.Symbol &&
                    (w as CsvSummaryWriter<MinuteSummary>)?.Interval == interval);

                var logger = loggerFactory.CreateLogger<SummaryProcessingService>();

                return new SummaryProcessingService(
                    summaryReader: reader,
                    multiMinuteAggregator: aggregator,
                    summaryWriter: writer,
                    pathProvider: pathProvider,
                    symbol: pair.Symbol,
                    targetTimeframe: pair.Timeframe,
                    logger: logger
                );
            }).ToList();
        });

        return services;
    }
}





