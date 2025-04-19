using Binance.Net.Clients;

namespace KriptoScraper.BinanceTrackers;
public class BinanceSolanaTracker
{
    private readonly PriceMonitorService _monitorService;
    private readonly string _symbol;

    public BinanceSolanaTracker(PriceMonitorService monitorService, string symbol)
    {
        _monitorService = monitorService;
        _symbol = symbol;
    }

    public async Task StartAsync()
    {
        var socketClient = new BinanceSocketClient();

        var result = await socketClient.SpotApi.ExchangeData.SubscribeToTickerUpdatesAsync(_symbol, async data =>
        {
            var now = DateTime.Now;
            var price = data.Data.LastPrice;

            await _monitorService.MonitorPriceAsync(_symbol, price);
        });

        if (!result.Success)
        {
            Console.WriteLine("❌ Binance verisi alınamadı: " + result.Error);
        }
    }
}


