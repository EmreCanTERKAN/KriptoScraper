using KriptoScraper.Domain.Entities;

namespace KriptoScraper.Domain.Interfaces;
public interface ITradeAggregator<TSummary>
{
    IEnumerable<TSummary> Aggregate(IEnumerable<TradeEvent> trades);
}
