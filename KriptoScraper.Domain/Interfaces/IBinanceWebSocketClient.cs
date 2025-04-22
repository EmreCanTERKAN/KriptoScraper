using KriptoScraper.Domain.Entities;

namespace KriptoScraper.Domain.Interfaces;
public interface IBinanceWebSocketClient
{
    Task SubscribeToTradeEventsAsync(string symbol, Func<TradeEvent, Task> onMessage);
}
