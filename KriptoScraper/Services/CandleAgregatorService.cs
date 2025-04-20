using KriptoScraper.Models;

namespace KriptoScraper.Services;
public class CandleAggregator
{
    private readonly TimeSpan _timeframe;
    private Candle _currentCandle;

    public event Func<Candle, Task> OnCandleClosed;

    public CandleAggregator(TimeSpan timeframe)
    {
        _timeframe = timeframe;
    }

    public async Task AddPriceAsync(decimal price, DateTime timestamp)
    {
        var periodStart = new DateTime(
            timestamp.Year,
            timestamp.Month,
            timestamp.Day,
            timestamp.Hour,
            timestamp.Minute / _timeframe.Minutes * _timeframe.Minutes,
            0);

        if (_currentCandle == null || periodStart > _currentCandle.OpenTime)
        {
            if (_currentCandle != null && OnCandleClosed != null)
                await OnCandleClosed.Invoke(_currentCandle);

            _currentCandle = new Candle
            {
                OpenTime = periodStart,
                Open = price,
                High = price,
                Low = price,
                Close = price,
                Volume = 0m
            };
        }
        else
        {
            _currentCandle.High = Math.Max(_currentCandle.High, price);
            _currentCandle.Low = Math.Min(_currentCandle.Low, price);
            _currentCandle.Close = price;
        }
    }
}

