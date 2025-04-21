using KriptoScraper.Dtos;

namespace KriptoScraper.Interfaces.Tracking;
public interface IBinanceWebSocketClient
{
    Task SubscribeToTradeEventsAsync(string symbol, Func<TradeEventDto, Task> onMessage);
}
