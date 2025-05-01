using CsvHelper.Configuration;
using KriptoScraper.Domain.Entities;
using System.Globalization;

namespace KriptoScraper.Application.Mappings;

public class KlineEventMap  : ClassMap<KlineEvent>
{
    public KlineEventMap ()
    {
        Map(m => m.Symbol).Name("symbol");
        Map(m => m.OpenTime).Name("open_time");
        Map(m => m.Open).Name("open");
        Map(m => m.High).Name("high");
        Map(m => m.Low).Name("low");
        Map(m => m.Close).Name("close");
        Map(m => m.Volume).Name("volume");
        Map(m => m.CloseTime).Name("close_time");
        Map(m => m.NumberOfTrades).Name("trades");
    }
}