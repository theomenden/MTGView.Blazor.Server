using MTGView.Data.Background.Interfaces;

namespace MTGView.Data.Background;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(IServiceProvider services,
        ILogger<Worker> logger)
    {
        Services = services;
        _logger = logger;
    }

    public IServiceProvider Services { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Consume Scoped Service Hosted Service running.");

        await ConsumeFileDownloadingServices(stoppingToken);

        await ConsumeDeserializationServices(stoppingToken);
    }

    private async Task ConsumeFileDownloadingServices(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Consume Scoped Service Hosted Service is working.");

        using var scope = Services.CreateScope();

        var scopedProcessingService = scope.ServiceProvider
                .GetRequiredService<IUnzippingService>();

        await scopedProcessingService.InitiateFileDownloadProcess(stoppingToken);
    }

    private async Task ConsumeDeserializationServices(CancellationToken stoppingToken)
    {
        const string allPrintingsFileName = "AllPrintings.json";

        using var scope = Services.CreateScope();

        var scopedProcessingService = scope.ServiceProvider
            .GetRequiredService<IDeserializationService>();

        var cards = await scopedProcessingService.DeserializeFileToMagicCardsAsync(allPrintingsFileName, stoppingToken).ToListAsync(stoppingToken);
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Consume Scoped Service Hosted Service is stopping.");

        await base.StopAsync(stoppingToken);
    }
}