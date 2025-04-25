using KriptoScraper.Domain;
using KriptoScraper.Worker;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // Burada ba��ml�l�klar�n�z� ekleyebilirsiniz.
        services.AddDomain(); // Kendi DomainRegistrar s�n�f�n�z� buraya ekleyin.
        services.AddHostedService<Worker>(); // Worker s�n�f�n�z� kaydedin.
    })
    .Build();

await host.RunAsync();

