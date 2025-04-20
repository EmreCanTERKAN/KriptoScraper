namespace KriptoScraper.Interfaces.DataStorage;
public interface ICsvService
{
    Task WriteToCsvAsync<T>(T data, string filePath);
}
