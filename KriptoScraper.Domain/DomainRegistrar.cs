using KriptoScraper.Domain.Entities;
using KriptoScraper.Domain.Interfaces;
using KriptoScraper.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KriptoScraper.Domain;
public static class DomainRegistrar
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddSingleton<ITradeAggregator<MinuteSummary>, MinuteTradeAggregator>();
        return services;
    }
}
