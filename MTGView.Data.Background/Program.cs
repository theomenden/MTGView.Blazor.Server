using MTGView.Data.Background;
using MTGView.Data.Background.Extensions;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(app => app.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true, true)
        .AddEnvironmentVariables())
    .ConfigureLogging(options => options.ClearProviders()
        .AddConsole())
    .ConfigureServices((context, services) =>
    {
        var connectionStrings = new Dictionary<string, string>
        {
            {"MtgApi", context.Configuration.GetConnectionString("MtgApi")},
            {"MtgDb", context.Configuration.GetConnectionString("MtgDb")}
        };

        services.AddBackgroundProcessingServices(connectionStrings);
        services.AddHostedService<Worker>();

    })
    .Build();

await host.RunAsync();
