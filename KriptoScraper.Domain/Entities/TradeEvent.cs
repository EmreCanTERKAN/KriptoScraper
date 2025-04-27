using KriptoScraper.Domain.Enums;

namespace KriptoScraper.Application.Entities;
public record TradeEvent(
    string Symbol,
    Timeframe TimeFrame,
    decimal Price,
    decimal Quantity,
    DateTime EventTimeUtc,
    DateTime ReceiveTimeUtc,
    bool IsBuyerMaker
);
