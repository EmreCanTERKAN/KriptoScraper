using KriptoScraper.Models;

namespace KriptoScraper.Interfaces.DataStorage;
public interface ITradeAggregator
{
    List<MinuteSummary> AggregateToMinuteSummary(List<TradeEvent> trades);
}
