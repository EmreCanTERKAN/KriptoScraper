namespace KriptoScraper.Models;
public class MinuteSummary
{
    public DateTime Minute { get; set; }
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public decimal Volume { get; set; }
    public int TradeCount { get; set; }
    public double BuyerMakerRatio { get; set; }
}
