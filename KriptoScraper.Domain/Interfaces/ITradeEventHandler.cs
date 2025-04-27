using KriptoScraper.Application.Entities;

namespace KriptoScraper.Application.Interfaces;
public interface ITradeEventHandler
{
    Task HandleAsync(TradeEvent tradeEvent);
}
