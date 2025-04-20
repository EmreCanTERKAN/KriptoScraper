using KriptoScraper.Interfaces;

public class PriceMonitorService(
    ILoggerService logger) : IPriceMonitorService
{
    private DateTime? lastLoggedSecond = null;
    private decimal? lastLoggedPrice = null;
    private decimal? latestSeenPriceInThisSecond = null;

    public async Task MonitorPriceAsync(string symbol, decimal price)
    {
        var now = DateTime.UtcNow; 
        var currentSecond = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

        // Fiyatın yeni bir saniyeye geçtiği durum
        if (lastLoggedSecond == null || lastLoggedSecond != currentSecond)
        {
            lastLoggedSecond = currentSecond;
            lastLoggedPrice = price;
            latestSeenPriceInThisSecond = price;

            await logger.LogAsync(symbol, now, price);
            return;
        }

        // Fiyat değişmişse ama küçük değişiklikler önlenmişse
        if (Math.Abs(price - latestSeenPriceInThisSecond!.Value) >= 0.001m)
        {
            latestSeenPriceInThisSecond = price;
            lastLoggedPrice = price;

            await logger.LogAsync(symbol, now, price);
        }
    }
}

