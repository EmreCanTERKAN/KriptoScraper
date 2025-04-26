using KriptoScraper.Domain.Interfaces;

namespace KriptoScraper.Domain.Services;
public class DefaultLogFilePathProvider : ILogFilePathProvider
{
    public string GetPath(string symbol, string interval, string fileType)
    {
        var baseDir = Path.Combine(Directory.GetCurrentDirectory(), "logs", symbol, interval);
        Directory.CreateDirectory(baseDir);
        return Path.Combine(baseDir, $"{symbol}_{interval}_{fileType}.csv");
    }
}
