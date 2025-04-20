namespace KriptoScraper.Interfaces;
public interface IPriceMonitorService<TLog>
{
    Task MonitorPriceAsync(string symbol, decimal price);
}
