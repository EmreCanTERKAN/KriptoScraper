using KriptoScraper.Domain.Entities;

namespace KriptoScraper.Application.Interfaces;
public interface ITradeEventProcessor
{
    Task ProcessAsync(TradeEvent tradeEvent);
}
