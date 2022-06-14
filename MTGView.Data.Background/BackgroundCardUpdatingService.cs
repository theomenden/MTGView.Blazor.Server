using MTGView.Data.Background.Interfaces;
using MTGView.Data.Background.Models;

namespace MTGView.Data.Background;

public class BackgroundCardUpdatingService : BackgroundService
{
    private readonly ILogger<BackgroundCardUpdatingService> _logger;
    
    private readonly IServiceProvider _services;

    public BackgroundCardUpdatingService(IServiceProvider services,
        ILogger<BackgroundCardUpdatingService> logger)
    {
        _services = services;
        _logger = logger;
    }

    
    private async Task ConsumeFileDownloadingServices(CancellationToken stoppingToken)
    {
        using var scope = _services.CreateScope();

        var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IUnzippingService>();

        await scopedProcessingService.InitiateFileDownloadProcess(stoppingToken);
    }

    private async Task ConsumeCardReplacementServices(CancellationToken stoppingToken)
    {
        using var scope = _services.CreateScope();

        var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IReplaceCardsService>();

        _logger.LogInformation("Processing started at {timeStarted}", DateTime.Now);

        await scopedProcessingService.DeserializeCsvToMagicCards($"{FileNamesToProcess.Cards}", stoppingToken);

        _logger.LogInformation("Processing finished at {timeStarted}", DateTime.Now);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await ConsumeFileDownloadingServices(stoppingToken);

        await ConsumeCardReplacementServices(stoppingToken);

        await CleanupDownloadedFiles(stoppingToken);

        await StopAsync(stoppingToken);
    }

    private static Task CleanupDownloadedFiles(CancellationToken stoppingToken)
    {
        const string pattern = "*.csv";

        var currentDirectory = Directory.GetCurrentDirectory();

        var matches = Directory.GetFiles(currentDirectory, pattern);

        foreach (var file in Directory.GetFiles(currentDirectory).Where(fileName => matches.Contains(fileName)))
        {
            File.Delete(file);
        }

        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        await base.StopAsync(stoppingToken);
    }
}