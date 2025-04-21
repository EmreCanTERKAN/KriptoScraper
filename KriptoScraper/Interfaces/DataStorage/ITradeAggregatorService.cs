using KriptoScraper.Models;

namespace KriptoScraper.Interfaces.DataStorage;
public interface ITradeAggregatorService
{
    void AddTrade(TradeEvent tradeEvent); // dış dünyadan gelenler buradan girer
    Task StartAsync(CancellationToken cancellationToken = default);
}
