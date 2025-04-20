using KriptoScraper.Interfaces;

namespace KriptoScraper.Services;
public class LoggerService<T>(
    ICsvService csvService) : ILoggerService<T>
{
    public async Task LogAsync(string symbol, DateTime time, T data)
    {
        Console.WriteLine($"{time:yyyy-MM-dd HH:mm:ss} - 📈 {symbol} Log Verisi: {data!.ToString()} $");

        var folder = Path.Combine("logs", typeof(T).Name.ToLower());
        Directory.CreateDirectory(folder);

        var fileName = $"{time:dd.MM.yyyy}-{symbol.ToLower()}_log.csv";
        var filePath = Path.Combine(folder, fileName);

        await csvService.WriteToCsvAsync(data, filePath);

    }
}

