using KriptoScraper.BinanceTrackers;
using KriptoScraper.Interfaces.DataStorage;
using KriptoScraper.Interfaces.Logging;
using KriptoScraper.Interfaces.Tracking;
using KriptoScraper.Models;
using KriptoScraper.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KriptoScraper;
public static class DependencyInjection
{
    public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        // Genel servisleri
        services.AddSingleton<ICsvService, CsvService>();

        // Solana için özel tanımlar
        services.AddSingleton<ILoggerService<SolanaLog>>(sp =>
        {
            var csv = sp.GetRequiredService<ICsvService>();
            return new LoggerService<SolanaLog>(csv);
        });

        services.AddSingleton<IPriceMonitorService<SolanaLog>>(sp =>
        {
            var logger = sp.GetRequiredService<ILoggerService<SolanaLog>>();
            return new PriceMonitorService<SolanaLog>(
                logger,
                (time, price) => new SolanaLog
                {
                    Timestamp = time,
                    Price = price
                });
        });

        services.AddSingleton<IBinanceTracker<SolanaLog>>(sp =>
        {
            var monitor = sp.GetRequiredService<IPriceMonitorService<SolanaLog>>();
            return new BinanceTracker<SolanaLog>(monitor, "SOLUSDT");
        });

        // Ethereum için özel tanımlar
        services.AddSingleton<ILoggerService<EthereumLog>>(sp =>
        {
            var csv = sp.GetRequiredService<ICsvService>();
            return new LoggerService<EthereumLog>(csv);
        });

        services.AddSingleton<IPriceMonitorService<EthereumLog>>(sp =>
        {
            var logger = sp.GetRequiredService<ILoggerService<EthereumLog>>();
            return new PriceMonitorService<EthereumLog>(
                logger,
                (time, price) => new EthereumLog
                {
                    Timestamp = time,
                    Price = price
                });
        });

        services.AddSingleton<IBinanceTracker<EthereumLog>>(sp =>
        {
            var monitor = sp.GetRequiredService<IPriceMonitorService<EthereumLog>>();
            return new BinanceTracker<EthereumLog>(monitor, "ETHUSDT");
        });
    }
}

