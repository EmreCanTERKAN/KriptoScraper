using KriptoScraper.Application.Entities;
using KriptoScraper.Domain.Enums;

namespace KriptoScraper.Application.Interfaces;
public interface IBinanceWebSocketClient
{
    Task SubscribeToTradeEventsAsync(string symbol, Timeframe timeframe, Func<TradeEvent, Task> onMessage);
}