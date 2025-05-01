using Binance.Net.Clients;
using Binance.Net.Interfaces.Clients;
using KriptoScraper.Application.Handlers;
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
        services.AddSingleton<IKlineEventHandler, KlineEventHandler>();
        services.AddSingleton<IKlineLoggerService, KlineLoggerService>();
        services.AddSingleton<IKlineEventWriter, CsvKlineEventWriter>();
        services.AddSingleton<ILogFilePathProvider, DefaultLogFilePathProvider>();
        services.AddSingleton<IBinanceSocketClient>(sp => 
            new BinanceSocketClient(options =>
            {
                
            }));
        return services;
    }
}





