using KriptoScraper.Domain.Entities;

namespace KriptoScraper.Application.Interfaces;
public interface ITradeBuffer
{
    void Add(TradeEvent trade);
    List<TradeEvent> Drain();
}
