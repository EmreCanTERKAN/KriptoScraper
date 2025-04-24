using KriptoScraper.Domain.Entities;

namespace KriptoScraper.Domain.Interfaces;
public interface ITradeEventProcessor
{
    Task ProcessAsync(TradeEvent tradeEvent);
}
