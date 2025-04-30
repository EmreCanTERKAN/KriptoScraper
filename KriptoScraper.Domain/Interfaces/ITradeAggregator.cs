using KriptoScraper.Domain.Entities;

namespace KriptoScraper.Application.Interfaces;
public interface ITradeAggregator<TSummary>
{
    IEnumerable<TSummary> Aggregate(IEnumerable<TradeEvent> trades);
}
