using KriptoScraper.Domain.Entities;

namespace KriptoScraper.Application.Interfaces;
public interface ITradeEventHandler
{
    Task HandleAsync(TradeEvent tradeEvent);
}
