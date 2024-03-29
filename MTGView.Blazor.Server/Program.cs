using Blazored.LocalStorage;
using Blazored.SessionStorage;
using ElectronNET.API;
using MTGView.Blazor.Server.Middleware;
using MTGView.Blazor.Server.Models;
using MTGView.Data.Background;
using MTGView.Data.Background.Extensions;
using TheOmenDen.Shared.Logging.Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
    .Enrich.FromLogContext()
    .Enrich.WithThreadId()
    .Enrich.WithProcessName()
    .Enrich.WithEnvironmentUserName()
    .Enrich.WithMemoryUsage()
    .WriteTo.Async(a =>
    {
        a.File("./logs/log-.txt", rollingInterval: RollingInterval.Day);
        a.Console();
    })
    .CreateBootstrapLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host
        .ConfigureAppConfiguration((context, config) =>
        {
            config
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            Task.Run(async () => await Electron.WindowManager.CreateWindowAsync());
        })
        .UseDefaultServiceProvider(options => options.ValidateScopes = false)
        .UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services));

    builder.WebHost.UseElectron(args);

    builder.Logging
        .ClearProviders()
        .AddSerilog();

    // Add services to the container.
    builder.Services
        .AddBlazorise(options => { options.Immediate = true; })
        .AddBootstrap5Providers()
        .AddBootstrap5Components()
        .AddBootstrapIcons()
        .AddBlazoriseRichTextEdit();

    builder.Services.AddLocalization();

    builder.Services.AddLazyCache();

    builder.Services.AddBlazoredLocalStorage();

    builder.Services.AddBlazoredSessionStorage();

    builder.Services.AddScoped<IModuleFactory, EsModuleFactory>();
    builder.Services.AddScoped<MtgIndexedDb>();
    builder.Services.AddScoped<SetInformationRepository>();
    builder.Services.AddScoped<SymbologyRepository>();
    builder.Services.AddSingleton<BackgroundUpdatingService>();
    builder.Services.AddHostedService(provider => provider.GetRequiredService<BackgroundUpdatingService>());
    builder.Services.AddBackgroundProcessingServicesForBlazor(builder.Configuration.GetConnectionString("MtgApi"));

    builder.Services.AddResponseCompression(options =>
    {
        options.MimeTypes = new[] { MediaTypeNames.Application.Octet };
    });

    builder.Services.AddResponseCaching();

    builder.Services.AddMtgDataServices(builder.Configuration.GetConnectionString("MtgDb"));

    builder.Services.AddScryfallApiServices();

    builder.Services.AddSignalR();

    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor()
        .AddHubOptions(options =>
        {
            options.MaximumReceiveMessageSize = 104_857_600;
        });

    await using var app = builder.Build();

    app.UseResponseCompression();

    // Configure the HTTP request pipeline.
    app.UseEnvironmentMiddleware(app.Environment);
    app.UseSerilogRequestLogging(options => options.EnrichDiagnosticContext = RequestLoggingConfigurer.EnrichFromRequest);

    app.UseWebSockets();

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseRouting();

    app.UseApiExceptionHandler(options =>
    {
        options.AddResponseDetails = OptionsDelegates.UpdateApiErrorResponse;
        options.DetermineLogLevel = OptionsDelegates.DetermineLogLevel;
    });

    app.MapGet("/background", (BackgroundUpdatingService service) => new BackgroundUpdatingServiceState(service.IsEnabled, service.IsRunning));

    app.MapMethods("/background", new[] { "PATCH" },
        (BackgroundUpdatingServiceState state, BackgroundUpdatingService service) =>
        {
            service.IsEnabled = state.IsEnabled;
            service.IsRunning = state.IsRunning;
        });

    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal("An error occurred before {AppName} could launch: {@Ex}", nameof(MTGView), ex);
}
finally
{
    Log.CloseAndFlush();
}