namespace KriptoScraper.Application.Interfaces;
public interface ISummaryWriter<TSummary> where TSummary : ISummary
{
    Task WriteBatchAsync(IEnumerable<TSummary> summaries, CancellationToken cancellationToken = default);
}
