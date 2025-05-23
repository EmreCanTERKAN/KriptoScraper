using KriptoScraper.Infrastructure;
using KriptoScraper.Worker;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        services.AddInfrastructure(ctx.Configuration);

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();

