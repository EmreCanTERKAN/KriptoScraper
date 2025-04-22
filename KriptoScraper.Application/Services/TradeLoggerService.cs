using KriptoScraper.Application.Interfaces;
using KriptoScraper.Domain.Interfaces;

namespace KriptoScraper.Application.Services;
public class TradeLoggerService(
    IBinanceWebSocketClient webSocketClient,
    ITradeEventHandler tradeEventHandler) // Writer ve Aggregator yerine tek bir handler
{
    public async Task StartLoggingAsync(string symbol)
    {
        await webSocketClient.SubscribeToTradeEventsAsync(symbol, async tradeEvent =>
        {
            await tradeEventHandler.HandleAsync(tradeEvent); // Tüm iş bu handler’a gider
        });
    }
}
