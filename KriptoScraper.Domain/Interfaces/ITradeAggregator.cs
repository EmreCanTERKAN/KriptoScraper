using KriptoScraper.Domain.Entities;

namespace KriptoScraper.Domain.Interfaces;
public interface ITradeAggregator<TSummary>
{
    List<TSummary> Aggregate(List<TradeEvent> trades);
}
