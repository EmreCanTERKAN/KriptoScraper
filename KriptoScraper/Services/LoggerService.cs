using KriptoScraper.Interfaces;
using KriptoScraper.LogInfos;

namespace KriptoScraper.Services;
public class LoggerService(
    ICsvService csvService) : ILoggerService
{
    public async Task LogAsync(string symbol, DateTime time, decimal price)
    {
        Console.WriteLine($"{time:yyyy-MM-dd HH:mm:ss} - 📈 {symbol} Fiyatı (Binance): {price} $");

        var log = new SolanaLog
        {
            Timestamp = time,
            Price = price
        };

        var folder = "logs";
        Directory.CreateDirectory(folder);
        var fileName = $"{time:dd.MM.yyyy}-{symbol.ToLower()}_log.csv";
        var filePath = Path.Combine(folder, fileName);

        await csvService.WriteToCsvAsync(log, filePath);

    }
}

