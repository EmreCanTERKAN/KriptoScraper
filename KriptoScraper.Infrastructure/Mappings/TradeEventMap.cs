using CsvHelper.Configuration;
using KriptoScraper.Application.Entities;
using System.Globalization;

namespace KriptoScraper.Application.Mappings;

public class TradeEventMap : ClassMap<TradeEvent>
{
    public TradeEventMap()
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