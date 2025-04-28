namespace KriptoScraper.Domain.Interfaces;
public interface ISummaryAggregator<TSummary>
{
    IEnumerable<TSummary> Aggregate(IEnumerable<TSummary> summaries, TimeSpan interval);
}
