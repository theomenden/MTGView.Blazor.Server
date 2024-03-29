﻿using EFCore.BulkExtensions;

namespace MTGView.Data.Background.Internal;

internal sealed class ReplaceRulingsService: IReplaceRulingsService
{
    private readonly IDbContextFactory<MagicthegatheringDbContext> _dbContextFactory;

    private readonly ILogger<ReplaceRulingsService> _logger;
    
    public ReplaceRulingsService(IDbContextFactory<MagicthegatheringDbContext> dbContextFactory, ILogger<ReplaceRulingsService> logger)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    public async Task DeserializeCsvToRulings(String fileName, CancellationToken cancellationToken = default)
    {
        await using var fileStream = File.OpenRead($"{fileName}{FileExtensions.CsvExtension}");

        using var reader = new StreamReader(fileStream);

        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<RulingsExcelMap>();

        var startTime = DateTime.Now;

        _logger.LogInformation("Starting Database Update Process at: {timeNow}", startTime);

        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        await ClearRulings(context, cancellationToken);

        var bulkConfig = new BulkConfig
        {
            BatchSize = 2000,
            EnableStreaming = true
        };

        var rulings = await csv.GetRecordsAsync<Ruling>(cancellationToken).ToListAsync(cancellationToken);

        await context.BulkInsertOrUpdateAsync(rulings, bulkConfig, null, typeof(Ruling), cancellationToken);

        await context.BulkSaveChangesAsync(bulkConfig,null, cancellationToken);

        _logger.LogInformation("Finished Database Update Process in: {timeNow} seconds", (DateTime.Now - startTime).TotalSeconds);
    }

    private async Task ClearRulings(MagicthegatheringDbContext context, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cleaning Previous Rulings from Database at {timeStarted}", DateTime.Now);

        var cmd = $"TRUNCATE TABLE {AnnotationHelper.TableName(context.Rulings)}";

        await context.Database.ExecuteSqlRawAsync(cmd, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cleared Previous Rulings from Database at {timeEnded}", DateTime.Now);
    }
}