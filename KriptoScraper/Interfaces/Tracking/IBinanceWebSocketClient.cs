using KriptoScraper.Models;

namespace KriptoScraper.Interfaces.Tracking;
public interface IBinanceWebSocketClient
{
    Task SubscribeToTradeEventsAsync(string symbol, Func<TradeEvent, Task> onMessage);
}
