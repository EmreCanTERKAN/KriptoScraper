using Binance.Net.Clients;
using KriptoScraper.Interfaces.Tracking;

namespace KriptoScraper.BinanceTrackers;
public class BinanceTracker<TLog>(
    IPriceMonitorService<TLog> priceMonitorService,
    string symbol) : IBinanceTracker<TLog>
{
    public async Task StartAsync()
    {
        var socketClient = new BinanceSocketClient();

        var result = await socketClient.SpotApi.ExchangeData.SubscribeToTickerUpdatesAsync(symbol, async data =>
        {

            var price = data.Data.LastPrice;
            await priceMonitorService.MonitorPriceAsync(symbol, price);

        });

        if (!result.Success)
        {
            Console.WriteLine("❌ Binance verisi alınamadı: " + result.Error);
        }
    }
}


