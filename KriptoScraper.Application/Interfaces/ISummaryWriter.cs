using KriptoScraper.Domain.Interfaces;

namespace KriptoScraper.Application.Interfaces;
public interface ISummaryWriter<TSummary> where TSummary : ISummary
{
    Task WriteAsync(TSummary summary);
}
