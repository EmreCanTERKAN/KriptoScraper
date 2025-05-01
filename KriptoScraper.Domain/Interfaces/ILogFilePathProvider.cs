namespace KriptoScraper.Domain.Interfaces;
public interface ILogFilePathProvider
{
    string GetRawKlinesPath(string symbol);

}
