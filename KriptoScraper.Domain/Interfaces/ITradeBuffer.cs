using KriptoScraper.Application.Entities;

namespace KriptoScraper.Application.Interfaces;
public interface ITradeBuffer
{
    void Add(TradeEvent trade);
    List<TradeEvent> Drain();
}
