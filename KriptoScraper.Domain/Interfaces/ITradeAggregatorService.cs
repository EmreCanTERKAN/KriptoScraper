using KriptoScraper.Domain.Entities;

namespace KriptoScraper.Application.Interfaces;
public interface ITradeAggregatorService
{
    void AddTrade(TradeEvent tradeEvent); // dış dünyadan gelenler buradan girer
    Task StartAsync(CancellationToken cancellationToken = default);
}
