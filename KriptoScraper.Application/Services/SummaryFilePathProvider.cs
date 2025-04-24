using KriptoScraper.Application.Interfaces;
using KriptoScraper.Domain.Enums;

namespace KriptoScraper.Application.Services;
public class SummaryFilePathProvider : ISummaryFilePathProvider
{
    public string GetSummaryFilePath(string symbol, Timeframe timeframe)
    {
        var folder = Path.Combine("logs", symbol, "summaries");

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        var fileName = timeframe switch
        {
            Timeframe.Minute => "minute.csv",
            Timeframe.Minute5 => "minute5.csv",
            Timeframe.Minute15 => "minute15.csv",
            Timeframe.Hour => "hour.csv",
            _ => throw new ArgumentOutOfRangeException(nameof(timeframe))
        };

        return Path.Combine(folder, fileName);
    }
}

