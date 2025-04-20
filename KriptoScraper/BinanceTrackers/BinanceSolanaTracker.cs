using Binance.Net.Clients;
using KriptoScraper.Interfaces;

namespace KriptoScraper.BinanceTrackers;
public class BinanceSolanaTracker(
    IPriceMonitorService priceMonitorService,
    string symbol) : IBinanceSolanaTracker
{
    public async Task StartAsync()
    {
        var socketClient = new BinanceSocketClient();

        var result = await socketClient.SpotApi.ExchangeData.SubscribeToTickerUpdatesAsync(symbol, async data =>
        {
            var now = DateTime.Now;
            var price = data.Data.LastPrice;

            await priceMonitorService.MonitorPriceAsync(symbol, price);
        });

        if (!result.Success)
        {
            Console.WriteLine("❌ Binance verisi alınamadı: " + result.Error);
        }
    }
}


