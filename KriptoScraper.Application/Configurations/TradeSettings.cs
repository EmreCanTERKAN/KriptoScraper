using KriptoScraper.Domain.Enums;

namespace KriptoScraper.Application.Configurations;
public sealed class TradeSettings
{
    public List<TradePair> Pairs { get; set; } = new();
}

public sealed class TradePair
{
    public string Symbol { get; set; } = string.Empty;
    public Timeframe Timeframe { get; set; }
}