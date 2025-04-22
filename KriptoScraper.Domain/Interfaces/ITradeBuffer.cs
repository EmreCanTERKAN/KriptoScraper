using KriptoScraper.Domain.Entities;

namespace KriptoScraper.Interfaces.DataStorage;
public interface ITradeBuffer
{
    void Add(TradeEvent trade);
    List<TradeEvent> Drain();
}
