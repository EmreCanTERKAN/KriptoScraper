using KriptoScraper.Application.Interfaces;
using KriptoScraper.Application.Settings;
using Microsoft.Extensions.Options;

namespace KriptoScraper.Application.Services;
public class TradeLoggerService(
    IBinanceWebSocketClient webSocketClient,
    ITradeEventHandler tradeEventHandler,
    IOptions<TradeSettings> settings) : ITradeLoggerService
{
    public async Task StartLoggingAsync(CancellationToken cancellationToken = default)
    {
        var pairs = settings.Value.Pairs;

        foreach (var pair in pairs)
        {
            _ = webSocketClient.SubscribeToTradeEventsAsync(pair.Symbol, pair.Timeframe, async tradeEvent =>
            {
                await tradeEventHandler.HandleAsync(tradeEvent);
            });
        }

        await Task.CompletedTask;
    }
}