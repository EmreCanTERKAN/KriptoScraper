namespace KriptoScraper.Domain.Entities;
public record KlineEvent(
    string Symbol,
    DateTime OpenTime,
    decimal Open,
    decimal High,
    decimal Low,
    decimal Close,
    decimal Volume,
    DateTime CloseTime,
    int NumberOfTrades
);
