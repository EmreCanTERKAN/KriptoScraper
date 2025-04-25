using KriptoScraper.Domain;
using KriptoScraper.Worker;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // Burada baðýmlýlýklarýnýzý ekleyebilirsiniz.
        services.AddDomain(); // Kendi DomainRegistrar sýnýfýnýzý buraya ekleyin.
        services.AddHostedService<Worker>(); // Worker sýnýfýnýzý kaydedin.
    })
    .Build();

await host.RunAsync();

