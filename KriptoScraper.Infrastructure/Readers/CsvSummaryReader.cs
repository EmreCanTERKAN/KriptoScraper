using CsvHelper;
using CsvHelper.Configuration;
using KriptoScraper.Application.Entities;
using KriptoScraper.Domain.Interfaces;
using KriptoScraper.Infrastructure.Mappings;
using System.Globalization;
using System.Text;

/// <summary>
/// CSV özet dosyasından ya tüm satırları ya da sondan N satırı okuyarak MinuteSummary nesnesi listesi üretir.
/// </summary>
public class CsvSummaryReader : ISummaryReader<MinuteSummary>
{
    private readonly CsvConfiguration _csvConfig;
    private const int MaxAttempts = 3;

    public CsvSummaryReader()
    {
        _csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            ShouldQuote = _ => true,
            TrimOptions = TrimOptions.Trim,
            HasHeaderRecord = true
        };
    }

    public Task<IEnumerable<MinuteSummary>> ReadSummariesAsync(
        string filePath,
        CancellationToken cancellationToken)
        => ReadLastNSummariesAsync(filePath, null, cancellationToken);


    public async Task<IEnumerable<MinuteSummary>> ReadLastNSummariesAsync(
        string filePath,
        int? lastNLines,
        CancellationToken cancellationToken)
    {
        if (!File.Exists(filePath))
            return Array.Empty<MinuteSummary>();

        for (int attempt = 1; attempt <= MaxAttempts; attempt++)
        {
            try
            {
                // Header satırını oku
                string headerLine = await ReadHeaderLineAsync(filePath, cancellationToken);

                // Veri satırlarını al
                IEnumerable<string> dataLines = lastNLines.HasValue
                    ? ReadLastLines(filePath, lastNLines.Value)
                    : ReadAllLines(filePath);

                // Header + veri satırlarını birleştir
                var csvContent = new StringBuilder();
                csvContent.AppendLine(headerLine);
                foreach (var line in dataLines)
                {
                    csvContent.AppendLine(line);
                }

                // StringReader üzerinden CSV parse et
                var summaryList = new List<MinuteSummary>();
                using var stringReader = new StringReader(csvContent.ToString());
                using var csv = new CsvReader(stringReader, _csvConfig);
                csv.Context.RegisterClassMap<MinuteSummaryMap>();

                await foreach (var record in csv.GetRecordsAsync<MinuteSummary>().WithCancellation(cancellationToken))
                {
                    summaryList.Add(record);
                }

                return summaryList.OrderBy(s => s.Period).ToList();
            }
            catch (IOException) when (attempt < MaxAttempts)
            {
                await Task.Delay(100, cancellationToken);
            }
        }

        throw new IOException($"Dosya '{filePath}' {MaxAttempts} denemede okunamadı.");
    }

    private static async Task<string> ReadHeaderLineAsync(string filePath, CancellationToken cancellationToken)
    {
        await using var fileStream = new FileStream(
            filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.ReadWrite);
        using var reader = new StreamReader(fileStream);
        var header = await reader.ReadLineAsync().ConfigureAwait(false);
        return header ?? throw new InvalidDataException("CSV dosyasında header bulunamadı.");
    }

    private static IEnumerable<string> ReadAllLines(string filePath)
    {
        using var fileStream = new FileStream(
            filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.ReadWrite);
        using var reader = new StreamReader(fileStream);
        // İlk satırı header olarak atla
        reader.ReadLine();
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            yield return line;
        }
    }

    private static IEnumerable<string> ReadLastLines(string filePath, int lineCount)
    {
        var allLines = File.ReadAllLines(filePath).Skip(1).ToList();
        var lastLines = allLines.TakeLast(lineCount).ToList();

        Console.WriteLine("📄 [ReadLastLines] Dosyadan son {0} satır alındı:", lineCount);
        foreach (var line in lastLines)
        {
            Console.WriteLine("   🕒 " + line);
        }

        return lastLines;
    }
}
