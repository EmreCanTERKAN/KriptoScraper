using KriptoScraper.Domain.Entities;

namespace KriptoScraper.Domain.Interfaces;
public interface ITradeBuffer
{
    void Add(TradeEvent trade);
    List<TradeEvent> Drain();
}
