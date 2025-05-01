using KriptoScraper.Application.Interfaces;
using KriptoScraper.Application.Settings;
using KriptoScraper.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace KriptoScraper.Application.Services;
public class KlineLoggerService(
    IBinanceWebSocketClient webSocketClient,
    IKlineEventHandler tradeEventHandler,
    IOptions<TradeSettings> settings) : IKlineLoggerService
{
    public async Task StartLoggingAsync(CancellationToken cancellationToken = default)
    {
        var pairs = settings.Value.Pairs;

        foreach (var pair in pairs)
        {
            _ = webSocketClient.SubscribeToKline1mAsync(pair.Symbol, async tradeEvent =>
            {
                await tradeEventHandler.HandleAsync(tradeEvent);
            });
        }

        await Task.CompletedTask;
    }
}