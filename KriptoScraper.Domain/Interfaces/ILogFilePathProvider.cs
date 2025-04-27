namespace KriptoScraper.Domain.Interfaces;
public interface ILogFilePathProvider
{
    string GetRawTradesPath(string symbol);

    string GetSummariesFolder(string symbol);

    string GetSummaryFilePath(string symbol, TimeSpan interval, DateTime date);
}
