using KriptoScraper.Domain.Entities;

namespace KriptoScraper.Domain.Interfaces;
public interface ITradeEventHandler
{
    Task HandleAsync(TradeEvent tradeEvent);
}
