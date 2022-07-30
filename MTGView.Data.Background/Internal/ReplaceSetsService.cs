using EFCore.BulkExtensions;

namespace MTGView.Data.Background.Internal;
internal class ReplaceSetsService: IReplaceSetsService
{
    private readonly IDbContextFactory<MagicthegatheringDbContext> _dbContextFactory;

    private readonly ILogger<ReplaceSetsService> _logger;

    public ReplaceSetsService(IDbContextFactory<MagicthegatheringDbContext> dbContextFactory,
        ILogger<ReplaceSetsService> logger)
    {
        _logger = logger;
        _dbContextFactory = dbContextFactory;
    }

    public async Task DeserializeCsvToSets(CancellationToken cancellationToken = default)
    {
        await using var fileStream = File.OpenRead($"{FileNamesToProcess.Sets}{FileExtensions.CsvExtension}");

        using var reader = new StreamReader(fileStream);

        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<MagicSetExcelMap>();

        var startTime = DateTime.Now;

        _logger.LogInformation("Starting Database Update Process at: {timeNow}", startTime);

        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        await ClearSets(context, cancellationToken);

        var bulkConfig = new BulkConfig
        {
            BatchSize = 2000,
            EnableStreaming = true
        };

        var sets = await csv.GetRecordsAsync<MagicSet>(cancellationToken).ToListAsync(cancellationToken);

        await context.BulkInsertOrUpdateAsync(sets, bulkConfig, null, typeof(MagicSet), cancellationToken);

        _logger.LogInformation("Finished Database Update Process in: {timeNow} seconds", (DateTime.Now - startTime).TotalSeconds);
    }

    private async Task ClearSets(MagicthegatheringDbContext context, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cleaning Previous sets from Database at {timeStarted}", DateTime.Now);

        var cmd = $"TRUNCATE TABLE {AnnotationHelper.TableName(context.Sets)}";

        await context.Database.ExecuteSqlRawAsync(cmd, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cleared Previous sets from Database at {timeEnded}", DateTime.Now);
    }
}