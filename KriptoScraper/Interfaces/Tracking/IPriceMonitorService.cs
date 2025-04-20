namespace KriptoScraper.Interfaces.Tracking;
public interface IPriceMonitorService<TLog>
{
    Task MonitorPriceAsync(string symbol, decimal price);
}
