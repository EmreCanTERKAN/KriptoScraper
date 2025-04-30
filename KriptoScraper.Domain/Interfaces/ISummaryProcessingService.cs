using KriptoScraper.Domain.Enums;

namespace KriptoScraper.Domain.Interfaces;
public interface ISummaryProcessingService
{
    Task ProcessAsync( CancellationToken cancellationToken = default);
}
