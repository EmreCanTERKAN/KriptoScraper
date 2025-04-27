using KriptoScraper.Domain.Buffers;
using KriptoScraper.Domain.Entities;
using KriptoScraper.Domain.Helpers;
using KriptoScraper.Domain.Interfaces;
using KriptoScraper.Domain.Settings;
using KriptoScraper.Infrastructure.WebSocketClients;
using KriptoScraper.Infrastructure.Writers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace KriptoScraper.Infrastructure;
public static class InfrastructureRegistrar
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services , IConfiguration configuration)
    {
        services.Configure<TradeSettings>(configuration.GetSection("TradeSettings"));


        services.AddSingleton<ITradeBuffer, TradeBuffer>();
        services.AddSingleton<IBinanceWebSocketClient, BinanceWebSocketClient>();
        services.AddSingleton<ITradeBuffer, TradeBuffer>();
        services.AddSingleton<ISummaryWriter<MinuteSummary>>(provider =>
        {
            var pathProvider = provider.GetRequiredService<ILogFilePathProvider>();
            var tradeSettings = provider.GetRequiredService<IOptions<TradeSettings>>().Value;

            return new CsvSummaryWriter<MinuteSummary>(
                pathProvider,
                tradeSettings.Pairs.First().Symbol,
                tradeSettings.Pairs.First().Timeframe.ToString()
            );
        });



        return services;
    }
}
