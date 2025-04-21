namespace KriptoScraper.Models;
public record TradeEvent(
    string Symbol,
    decimal Price,
    decimal Quantity,
    DateTime EventTimeUtc,
    DateTime ReceiveTimeUtc,
    bool IsBuyerMaker
);
