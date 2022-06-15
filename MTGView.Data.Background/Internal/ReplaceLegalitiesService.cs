using System.Globalization;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using MTGView.Core.Models;
using MTGView.Core.Mapping.ExcelMappings;
using MTGView.Data.Background.Helpers;
using MTGView.Data.Background.Interfaces;
using MTGView.Data.EFCore.Contexts;

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
        await ClearLegalities();

        await using var fileStream = new FileStream($"{fileName}.{FileExtension}", FileMode.Open, FileAccess.Read);

        using var reader = new StreamReader(fileStream);

        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<LegalityExcelMap>();

        var startTime = DateTime.Now;

        _logger.LogInformation("Starting Database Update Process at: {timeNow}", startTime);

        var legalities = new List<Legality>(70_000);

        legalities.AddRange(csv.GetRecords<Legality>());

        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        await context.BulkInsertAllAsync(legalities, cancellationToken);

        _logger.LogInformation("Finished Database Update Process in: {timeNow} seconds", (DateTime.Now - startTime).TotalSeconds);
    }

    private async Task ClearLegalities()
    {
        _logger.LogInformation("Cleaning Previous legalities from Database at {timeStarted}", DateTime.Now);

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var cmd = $"TRUNCATE TABLE {AnnotationHelper.TableName(context.Legalities)}";

        await context.Database.ExecuteSqlRawAsync(cmd);

        await context.SaveChangesAsync();

        _logger.LogInformation("Cleared Previous legalities from Database at {timeEnded}", DateTime.Now);
    }

}
