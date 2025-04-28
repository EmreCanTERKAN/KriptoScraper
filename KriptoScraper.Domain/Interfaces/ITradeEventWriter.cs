using KriptoScraper.Application.Entities;

namespace KriptoScraper.Domain.Interfaces;
public interface ITradeEventWriter
{
    Task WriteAsync(string symbol, TradeEvent tradeEvent);
}
