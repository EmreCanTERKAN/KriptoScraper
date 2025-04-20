using KriptoScraper.Interfaces;

public class PriceMonitorService<TLog>(
    ILoggerService<TLog> logger,
    Func<DateTime, decimal, TLog> logFactory
) : IPriceMonitorService<TLog>
{
    private DateTime? lastLoggedSecond = null;
    private decimal? latestSeenPriceInThisSecond = null;

    public async Task MonitorPriceAsync(string symbol, decimal price)
    {
        var now = DateTime.UtcNow;
        var currentSecond = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

        var log = logFactory(now, price); // 💡 Generic log objesi burada yaratılıyor

        if (lastLoggedSecond == null || lastLoggedSecond != currentSecond)
        {
            lastLoggedSecond = currentSecond;
            latestSeenPriceInThisSecond = price;

            await logger.LogAsync(symbol, now, log);
            return;
        }

        if (Math.Abs(price - latestSeenPriceInThisSecond!.Value) >= 0.001m)
        {
            latestSeenPriceInThisSecond = price;

            await logger.LogAsync(symbol, now, log);
        }
    }
}

