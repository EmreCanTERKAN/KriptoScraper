using KriptoScraper.Domain.Interfaces;

namespace KriptoScraper.Application.Services;
public class DefaultLogFilePathProvider : ILogFilePathProvider
{
    public string GetPath(string symbol, TimeSpan interval, string fileType)
    {
        var intervalDir = $"{interval.Hours:D2}h{interval.Minutes:D2}m";
        var baseDir = Path.Combine(Directory.GetCurrentDirectory(), "logs", symbol, intervalDir);
        Directory.CreateDirectory(baseDir);
        return Path.Combine(baseDir, $"{symbol}_{intervalDir}_{fileType}.csv");
    }
}
