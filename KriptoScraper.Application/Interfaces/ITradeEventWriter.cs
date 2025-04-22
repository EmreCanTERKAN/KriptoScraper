using KriptoScraper.Domain.Entities;

namespace KriptoScraper.Interfaces.DataStorage;
public interface ITradeEventWriter
{
    Task WriteAsync(TradeEvent tradeEvent);
}
