using KriptoScraper.Interfaces;
using KriptoScraper.Models;

namespace KriptoScraper.Services;
public class CandleLoggerService(
    ICsvService csvService,
    string symbol)
{
    public Task HandleCandleAsync(Candle candle)
    {
        var folder = "candles";
        Directory.CreateDirectory(folder);

        var fileName = $"{symbol.ToLower()}_candles.csv";
        var filePath = Path.Combine(folder, fileName);

        var record = new
        {
            Time = candle.OpenTime,
            Open = candle.Open,
            High = candle.High,
            Low = candle.Low,
            Close = candle.Close,
            Volume = candle.Volume
        };

        return csvService.WriteToCsvAsync(record, filePath);
    }
}
