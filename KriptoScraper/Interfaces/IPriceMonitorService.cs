namespace KriptoScraper.Interfaces;
public interface IPriceMonitorService
{
    Task MonitorPriceAsync(string symbol, decimal price);
}
