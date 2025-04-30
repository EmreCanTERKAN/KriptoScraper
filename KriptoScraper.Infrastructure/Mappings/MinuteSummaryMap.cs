using CsvHelper.Configuration;
using KriptoScraper.Application.Entities;
using System.Globalization;

namespace KriptoScraper.Infrastructure.Mappings;
public class MinuteSummaryMap : ClassMap<MinuteSummary>
{
    public MinuteSummaryMap()
    {
        Map(x => x.Period)
            .Name("Period")
            .TypeConverterOption.Format("MM/dd/yyyy HH:mm:ss");

        Map(x => x.Open)
            .Name("Open")
            .TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);

        Map(x => x.High)
            .Name("High")
            .TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);

        Map(x => x.Low)
            .Name("Low")
            .TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);

        Map(x => x.Close)
            .Name("Close")
            .TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);

        Map(x => x.Volume)
            .Name("Volume")
            .TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);

        Map(x => x.TradeCount)
            .Name("TradeCount");

        Map(x => x.BuyerMakerRatio)
            .Name("BuyerMakerRatio")
            .TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);
    }
}
