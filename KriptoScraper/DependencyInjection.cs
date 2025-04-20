using KriptoScraper.BinanceTrackers;
using KriptoScraper.Interfaces;
using KriptoScraper.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KriptoScraper;
public static class DependencyInjection
{
    public static void ConfigureServices(HostBuilderContext context,IServiceCollection services)
    {
        services.AddSingleton<ICsvService, CsvService>();
        services.AddSingleton<ILoggerService, LoggerService>();
        services.AddSingleton<IPriceMonitorService, PriceMonitorService>();

        services.AddSingleton<IBinanceSolanaTracker>(sp =>
        {
            var monitor = sp.GetRequiredService<IPriceMonitorService>();
            return new BinanceSolanaTracker(monitor, "SOLUSDT");
        });
       
    }
}
