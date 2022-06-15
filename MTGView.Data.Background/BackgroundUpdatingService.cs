using MTGView.Data.Background.Interfaces;
using MTGView.Data.Background.Models;

namespace MTGView.Data.Background;

public class BackgroundUpdatingService : BackgroundService
{
    private readonly ILogger<BackgroundUpdatingService> _logger;

    private readonly IServiceProvider _services;

    public BackgroundUpdatingService(IServiceProvider services,
        ILogger<BackgroundUpdatingService> logger)
    {
        _services = services;
        _logger = logger;
    }
    #region Overrides
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await ConsumeFileDownloadingServices(stoppingToken);

        await ConsumeCardReplacementServices(stoppingToken);

        await ConsumeRulingReplacementServices(stoppingToken);

        await ConsumeLegalityReplacementServices(stoppingToken);

        await CleanupDownloadedFiles(stoppingToken);

        await StopAsync(stoppingToken);
    }

    public override Task StopAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
    #endregion
    #region Cleanup
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
    #endregion
    #region Scoped Service Calls
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

    private async Task ConsumeRulingReplacementServices(CancellationToken stoppingToken)
    {
        using var scope = _services.CreateScope();

        var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IReplaceRulingsService>();

        _logger.LogInformation("Processing started at {timeStarted}", DateTime.Now);

        await scopedProcessingService.DeserializeCsvToRulings($"{FileNamesToProcess.Rulings}", stoppingToken);

        _logger.LogInformation("Processing finished at {timeStarted}", DateTime.Now);
    }

    private async Task ConsumeLegalityReplacementServices(CancellationToken stoppingToken)
    {
        using var scope = _services.CreateScope();

        var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IReplaceLegalitiesService>();

        _logger.LogInformation("Processing started at {timeStarted}", DateTime.Now);

        await scopedProcessingService.DeserializeCsvToLegalities($"{FileNamesToProcess.Legalities}", stoppingToken);

        _logger.LogInformation("Processing finished at {timeStarted}", DateTime.Now);
    }
    #endregion
}