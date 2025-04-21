using KriptoScraper.Interfaces.DataStorage;
using KriptoScraper.Interfaces.Tracking;

namespace KriptoScraper.Services.DataStorage;
public class TradeLoggerService(
    IBinanceWebSocketClient webSocketClient,
    ITradeEventWriter writer,
    ITradeAggregatorService aggregatorService)
{
    public async Task StartLoggingAsync(string symbol)
    {
        await webSocketClient.SubscribeToTradeEventsAsync(symbol, async tradeEvent =>
        {
            aggregatorService.AddTrade(tradeEvent);
            await writer.WriteAsync(tradeEvent);
        });
    }
}
