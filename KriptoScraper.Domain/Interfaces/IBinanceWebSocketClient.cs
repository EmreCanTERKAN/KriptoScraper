using KriptoScraper.Domain.Entities;
using KriptoScraper.Domain.Enums;

namespace KriptoScraper.Domain.Interfaces;
public interface IBinanceWebSocketClient
{
    Task SubscribeToTradeEventsAsync(string symbol, Timeframe timeframe, Func<TradeEvent, Task> onMessage);
}