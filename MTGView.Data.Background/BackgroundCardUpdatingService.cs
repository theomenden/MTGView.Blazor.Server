using MTGView.Data.Background.Interfaces;
using MTGView.Data.Background.Models;

namespace MTGView.Data.Background;

public class BackgroundCardUpdatingService : BackgroundService
{
    private readonly ILogger<BackgroundCardUpdatingService> _logger;

    public BackgroundCardUpdatingService(IServiceProvider services,
        ILogger<BackgroundCardUpdatingService> logger)
    {
        Services = services;
        _logger = logger;
    }

    public IServiceProvider Services { get; }
    
    private async Task ConsumeFileDownloadingServices(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Consume Scoped Service Hosted Service is working.");

        using var scope = Services.CreateScope();

        var scopedProcessingService = scope.ServiceProvider
                .GetRequiredService<IUnzippingService>();

        await scopedProcessingService.InitiateFileDownloadProcess(stoppingToken);
    }

    private async Task ConsumeCardReplacementServices(CancellationToken stoppingToken)
    {
        using var scope = Services.CreateScope();

        var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IReplaceCardsService>();

        _logger.LogInformation("Processing started at {timeStarted}", DateTime.Now);

        await scopedProcessingService.DeserializeCsvToMagicCards($"{FileNamesToProcess.Cards}", stoppingToken);

        _logger.LogInformation("Processing finished at {timeStarted}", DateTime.Now);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
    
        _logger.LogInformation("Consume Scoped Service Hosted Service running.");
     
        await ConsumeFileDownloadingServices(stoppingToken);
        await ConsumeCardReplacementServices(stoppingToken);

        CleanupDownloadedFiles();
    }

    private static void CleanupDownloadedFiles()
    {
        const string pattern = "*.csv";

        var matches = Directory.GetFiles(Directory.GetCurrentDirectory(), pattern);

        foreach (var file in Directory.GetFiles(Directory.GetCurrentDirectory()).Where(fileName => matches.Contains(fileName)))
        {
            File.Delete(file);
        }
    }

    public override Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Consume Scoped Service Hosted Service is stopping.");

        return Task.CompletedTask;
    }
}