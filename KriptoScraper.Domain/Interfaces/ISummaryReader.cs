using KriptoScraper.Application.Entities;

namespace KriptoScraper.Domain.Interfaces;
public interface ISummaryReader<TSummary>
{
    Task<IEnumerable<TSummary>> ReadSummariesAsync(string filePath, CancellationToken cancellationToken);
    Task<IEnumerable<MinuteSummary>> ReadLastNSummariesAsync(
        string filePath,
        int? lastNLines,
        CancellationToken cancellationToken);
}
