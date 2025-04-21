using CsvHelper.Configuration;
using KriptoScraper.Models;
using System.Globalization;

namespace KriptoScraper.Mapping;

public class TradeEventDtoMap : ClassMap<TradeEvent>
{
    public TradeEventDtoMap()
    {
        Map(x => x.Symbol).Name("Symbol");

        Map(x => x.Price)
            .Name("Price")
            .TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);

        Map(x => x.Quantity)
            .Name("Quantity")
            .TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);

        Map(x => x.EventTimeUtc)
            .Name("EventTimeUtc")
            .TypeConverterOption.Format("o"); // ISO 8601

        Map(x => x.ReceiveTimeUtc)
            .Name("ReceiveTimeUtc")
            .TypeConverterOption.Format("o");

        Map(x => x.IsBuyerMaker).Name("IsBuyerMaker");
    }
}