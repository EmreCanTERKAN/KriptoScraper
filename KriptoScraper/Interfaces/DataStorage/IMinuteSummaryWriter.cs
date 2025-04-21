using KriptoScraper.Models;

namespace KriptoScraper.Interfaces.DataStorage;
public interface IMinuteSummaryWriter
{
    Task WriteAsync(MinuteSummary summary);
}
