using KriptoScraper.Domain.Interfaces;

namespace KriptoScraper.Application.Services;
public class DefaultLogFilePathProvider : ILogFilePathProvider
{
    private static string ToIntervalString(TimeSpan interval) => $"{interval.Hours:D2}h{interval.Minutes:D2}m";
    /// <summary>
    /// "logs/{symbol}" klasörünü hazırlar ve raw trades dosya yolunu döner.
    /// </summary>
    public string GetRawTradesPath(string symbol)
    {
        var baseDir = Path.Combine(
            Directory.GetCurrentDirectory(),
            "logs",
            symbol);

        Directory.CreateDirectory(baseDir);
        return Path.Combine(baseDir, $"{symbol}_trades.csv");
    }

    public string GetSummariesFolder(string symbol)
    {
        return EnsureSymbolFolder(symbol, "summaries");
    }

    public string GetSummaryFilePath(string symbol, TimeSpan interval, DateTime date)
    {
        var baseSummaryDir = EnsureSymbolFolder(symbol, "summaries", ToIntervalString(interval));
        var datePart = date.ToString("yyyy-MM-dd");
        var fileName = $"{datePart}_summary.csv";
        return Path.Combine(baseSummaryDir, fileName);

    }

    private string EnsureSymbolFolder(string symbol,params string[] subPaths)
    {
        var parts = new[]
        {
            Directory.GetCurrentDirectory(),
            "logs",
            symbol
        }
        .Concat(subPaths)
        .ToArray();
        var path = Path.Combine(parts);
        Directory.CreateDirectory(path);
        return path;
    }
}
