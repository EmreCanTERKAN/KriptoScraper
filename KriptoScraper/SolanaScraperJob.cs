using Quartz;

public class SolanaScraperJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await SolanaScraper.GetSolanaPrice();
    }
}
