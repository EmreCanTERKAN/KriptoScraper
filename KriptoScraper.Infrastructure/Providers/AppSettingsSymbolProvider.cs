using KriptoScraper.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace KriptoScraper.Infrastructure.Services;
public class AppSettingsSymbolProvider : ISymbolProvider
{
    private readonly IConfiguration _configuration;

    public AppSettingsSymbolProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetSymbol()
    {
        return _configuration.GetValue<string>("TradeSettings:Symbol")!;
    }
}

