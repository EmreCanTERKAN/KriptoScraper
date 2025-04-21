using KriptoScraper.Dtos;
using KriptoScraper.Services.DataStorage;

public class CsvTradeEventWriterTests
{
    [Fact]
    public async Task WriteAsync_ShouldWriteCorrectCsvFormat()
    {
        // Arrange
        var testFilePath = "test_trade_output.csv";
        if (File.Exists(testFilePath))
            File.Delete(testFilePath);

        var tradeEvent = new TradeEventDto(
            Symbol: "BTCUSDT",
            Price: 12345.67m,
            Quantity: 0.01m,
            EventTimeUtc: new DateTime(2025, 4, 21, 12, 0, 0, DateTimeKind.Utc),
            ReceiveTimeUtc: new DateTime(2025, 4, 21, 12, 0, 1, DateTimeKind.Utc),
            IsBuyerMaker: true
        );

        var writer = new CsvTradeEventWriter(testFilePath);

        // Act
        await writer.WriteAsync(tradeEvent);

        // Assert
        var lines = File.ReadAllLines(testFilePath);

        Assert.Equal(2, lines.Length);
        Assert.Equal("Symbol,Price,Quantity,EventTimeUtc,ReceiveTimeUtc,IsBuyerMaker", lines[0]);
        Assert.Contains("BTCUSDT", lines[1]);
        Assert.Contains("12345.67", lines[1]);
        Assert.Contains("0.01", lines[1]);
        Assert.Contains("True", lines[1]);
    }
}
