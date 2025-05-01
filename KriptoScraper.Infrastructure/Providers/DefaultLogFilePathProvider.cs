using KriptoScraper.Domain.Interfaces;

namespace KriptoScraper.Application.Services;
public class DefaultLogFilePathProvider : ILogFilePathProvider
{
    public string GetRawKlinesPath(string symbol)
    {
        var dateString = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var fileName = $"{dateString}.csv";

        var folder = EnsureSymbolFolder(symbol, "klines");
        var fullPath = Path.Combine(folder, fileName);

        return fullPath;
    }

    private string EnsureSymbolFolder(string symbol, params string[] subPaths)
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
