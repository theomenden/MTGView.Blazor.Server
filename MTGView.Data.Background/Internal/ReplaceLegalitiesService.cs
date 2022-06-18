namespace MTGView.Data.Background.Internal;

internal sealed class ReplaceLegalitiesService: IReplaceLegalitiesService
{
    private readonly IDbContextFactory<MagicthegatheringDbContext> _dbContextFactory;

    private readonly ILogger<ReplaceLegalitiesService> _logger;

    private const string FileExtension = "csv";

    public ReplaceLegalitiesService(IDbContextFactory<MagicthegatheringDbContext> dbContextFactory,
        ILogger<ReplaceLegalitiesService> logger)
    {
        _logger = logger;
        _dbContextFactory = dbContextFactory;
    }

    public async Task DeserializeCsvToLegalities(string fileName, CancellationToken cancellationToken = default)
    {
        await using var fileStream = File.OpenRead($"{fileName}.{FileExtension}");

        using var reader = new StreamReader(fileStream);

        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<LegalityExcelMap>();

        var startTime = DateTime.Now;

        _logger.LogInformation("Starting Database Update Process at: {timeNow}", startTime);

        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        await ClearLegalities(context, cancellationToken);

        await context.BulkInsertStreamAsync(csv.GetRecordsAsync<Legality>(cancellationToken), cancellationToken);

        _logger.LogInformation("Finished Database Update Process in: {timeNow} seconds", (DateTime.Now - startTime).TotalSeconds);
    }

    private async Task ClearLegalities(MagicthegatheringDbContext context, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cleaning Previous legalities from Database at {timeStarted}", DateTime.Now);

        var cmd = $"TRUNCATE TABLE {AnnotationHelper.TableName(context.Legalities)}";

        await context.Database.ExecuteSqlRawAsync(cmd, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cleared Previous legalities from Database at {timeEnded}", DateTime.Now);
    }

}
