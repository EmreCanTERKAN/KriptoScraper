namespace KriptoScraper.Dtos;
public record TradeEventDto(
    string Symbol,
    decimal Price,
    decimal Quantity,
    DateTime EventTimeUtc,
    DateTime ReceiveTimeUtc,
    bool IsBuyerMaker
);
