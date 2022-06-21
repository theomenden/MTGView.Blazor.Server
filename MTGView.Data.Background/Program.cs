using var host = Host.CreateDefaultBuilder(args)
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
        services.AddHostedService<BackgroundUpdatingService>();
    })
    .Build();

await host.RunAsync();