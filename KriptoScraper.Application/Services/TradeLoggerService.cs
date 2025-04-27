using KriptoScraper.Application.Interfaces;
using KriptoScraper.Application.Settings;
using Microsoft.Extensions.Options;

namespace KriptoScraper.Application.Services;
public class TradeLoggerService(
    IBinanceWebSocketClient webSocketClient,
    ITradeEventHandler tradeEventHandler,
    IOptions<TradeSettings> settings) : ITradeLoggerService
{
    public async Task StartLoggingAsync()
    {
        var pairs = settings.Value.Pairs;

        var tasks = pairs.Select(pair => Task.Run(async () =>
        {
            // WebSocket aboneliği başlatılıyor ve işlem bekleniyor
            await webSocketClient.SubscribeToTradeEventsAsync(pair.Symbol, pair.Timeframe, async tradeEvent =>
            {
                await tradeEventHandler.HandleAsync(tradeEvent); // Veri işleme
            });
        })).ToList();

        // Tüm görevlerin tamamlanmasını bekleyelim
        await Task.WhenAll(tasks);
    }
}
