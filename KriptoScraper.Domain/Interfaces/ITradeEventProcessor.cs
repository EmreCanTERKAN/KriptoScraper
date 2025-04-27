using KriptoScraper.Application.Entities;

namespace KriptoScraper.Application.Interfaces;
public interface ITradeEventProcessor
{
    Task ProcessAsync(TradeEvent tradeEvent);
}
