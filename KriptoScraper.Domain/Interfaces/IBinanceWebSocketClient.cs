using KriptoScraper.Domain.Entities;

namespace KriptoScraper.Domain.Interfaces;
public interface IBinanceWebSocketClient
{
    Task SubscribeToKline1mAsync(string symbol, Func<KlineEvent, Task> onKlineReceived);
}