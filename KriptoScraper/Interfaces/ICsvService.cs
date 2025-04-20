using KriptoScraper.LogInfos;

namespace KriptoScraper.Interfaces;
public interface ICsvService
{
    Task WriteToCsvAsync(SolanaLog data, string filePath);
}
