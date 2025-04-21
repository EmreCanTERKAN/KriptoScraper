using KriptoScraper.Dtos;

namespace KriptoScraper.Interfaces.DataStorage;
public interface ITradeEventWriter
{
    Task WriteAsync(TradeEventDto tradeEvent);
}
