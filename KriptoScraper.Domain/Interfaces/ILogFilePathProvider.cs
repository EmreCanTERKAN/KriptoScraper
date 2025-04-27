namespace KriptoScraper.Domain.Interfaces;
public interface ILogFilePathProvider
{
    string GetPath(string symbol, TimeSpan interval, string fileType); // örn: "SOLUSDT", "1m", "summary"
}
