using KriptoScraper.Domain.Enums;

namespace KriptoScraper.Application.Interfaces;
public interface ISummaryFilePathProvider
{
    string GetSummaryFilePath(string symbol, Timeframe timeframe);
}
