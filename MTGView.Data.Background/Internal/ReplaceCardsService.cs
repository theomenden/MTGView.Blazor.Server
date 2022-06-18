﻿namespace MTGView.Data.Background.Internal;

internal sealed class ReplaceCardsService : IReplaceCardsService
{
    private readonly IDbContextFactory<MagicthegatheringDbContext> _dbContextFactory;

    private readonly ILogger<ReplaceCardsService> _logger;

    private const string FileExtension = "csv";

    public ReplaceCardsService(IDbContextFactory<MagicthegatheringDbContext> dbContextFactory,
        ILogger<ReplaceCardsService> logger)
    {
        _logger = logger;
        _dbContextFactory = dbContextFactory;
    }

    public async Task DeserializeCsvToMagicCards(String fileName, CancellationToken cancellationToken = default)
    {
        await using var fileStream = File.OpenRead($"{fileName}.{FileExtension}");

        using var reader = new StreamReader(fileStream);

        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<MagicCardExcelMap>();

        var startTime = DateTime.Now;

        _logger.LogInformation("Starting Database Update Process at: {timeNow}", startTime);

        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        await ClearCards(context, cancellationToken);

        await context.BulkInsertStreamAsync(csv.EnumerateRecordsAsync<MagicCard>(new(), cancellationToken), cancellationToken);

        _logger.LogInformation("Finished Database Update Process in: {timeNow} seconds", (DateTime.Now - startTime).TotalSeconds);
    }

    private async Task ClearCards(MagicthegatheringDbContext context, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cleaning Previous cards from Database at {timeStarted}", DateTime.Now);

        var cmd = $"TRUNCATE TABLE {AnnotationHelper.TableName(context.Cards)}";

        await context.Database.ExecuteSqlRawAsync(cmd, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cleared Previous cards from Database at {timeEnded}", DateTime.Now);
    }
}