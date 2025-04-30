using KriptoScraper.Application.Interfaces;

namespace KriptoScraper.Application.Entities;
public class MinuteSummary : ISummary
{
    public DateTime Period { get; set; }
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public decimal Volume { get; set; }
    public int TradeCount { get; set; }
    public double BuyerMakerRatio { get; set; }

    public MinuteSummary()
    {
        
    }

    public MinuteSummary(DateTime period, decimal open, decimal high, decimal low, decimal close, decimal volume, int tradeCount, double buyerMakerRatio)
    {
        Period = period;
        Open = open;
        High = high;
        Low = low;
        Close = close;
        Volume = volume;
        TradeCount = tradeCount;
        BuyerMakerRatio = buyerMakerRatio;
    }

}
