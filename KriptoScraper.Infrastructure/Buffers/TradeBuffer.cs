using KriptoScraper.Application.Entities;
using KriptoScraper.Application.Interfaces;

namespace KriptoScraper.Application.Buffers;
public class TradeBuffer : ITradeBuffer
{
    private readonly List<TradeEvent> _buffer = new();

    public void Add(TradeEvent trade)
    {
        lock (_buffer)
        {
            _buffer.Add(trade);
        }
    }

    public List<TradeEvent> Drain()
    {
        lock (_buffer)
        {
            var drained = new List<TradeEvent>(_buffer);
            _buffer.Clear();
            return drained;
        }
    }
}
