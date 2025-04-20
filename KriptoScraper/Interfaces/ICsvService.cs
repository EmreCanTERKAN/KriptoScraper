using KriptoScraper.LogInfos;

namespace KriptoScraper.Interfaces;
public interface ICsvService
{
    Task WriteToCsvAsync<T>(T data, string filePath);
}
