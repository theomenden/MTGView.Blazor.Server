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

    private List<Legality>? _legalities = new (50_000);

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

        await using var fileStream = File.OpenRead($"{fileName}.{FileExtension}");

        using var reader = new StreamReader(fileStream);

        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<LegalityExcelMap>();

        var startTime = DateTime.Now;

        _logger.LogInformation("Starting Database Update Process at: {timeNow}", startTime);

        _legalities = csv.GetRecords<Legality>().ToList();

        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        await context.BulkInsertAllAsync(_legalities, cancellationToken);

        _legalities.Clear();
        _legalities = null;

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
