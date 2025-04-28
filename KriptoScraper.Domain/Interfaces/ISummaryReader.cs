using KriptoScraper.Application.Entities;

namespace KriptoScraper.Domain.Interfaces;
public interface ISummaryReader<TSummary>
{
    Task<IEnumerable<TSummary>> ReadSummariesAsync(string filePath, CancellationToken cancellationToken);
}
