using KriptoScraper.Interfaces;
using KriptoScraper.Logs;

namespace KriptoScraper.Services;
public class CsvConsoleLogger : ILoggerService
{
    public Task LogAsync(string symbol, DateTime time, decimal price)
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

        var csvService = new CsvService();
        csvService.WriteToCsv(log, filePath);

        return Task.CompletedTask;
    }
}

