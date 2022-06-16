using System.Globalization;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using MTGView.Core.Mapping.ExcelMappings;
using MTGView.Core.Models;
using MTGView.Data.Background.Helpers;
using MTGView.Data.Background.Interfaces;
using MTGView.Data.EFCore.Contexts;

namespace MTGView.Data.Background.Internal;

internal sealed class ReplaceRulingsService: IReplaceRulingsService
{
    private readonly IDbContextFactory<MagicthegatheringDbContext> _dbContextFactory;

    private readonly ILogger<ReplaceRulingsService> _logger;

    private List<Ruling>? _rulings = new (50_000);

    private const string FileExtension = "csv";

    public ReplaceRulingsService(IDbContextFactory<MagicthegatheringDbContext> dbContextFactory, ILogger<ReplaceRulingsService> logger)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    public async Task DeserializeCsvToRulings(String fileName, CancellationToken cancellationToken = default)
    {
        await ClearRulings();

        await using var fileStream = File.OpenRead($"{fileName}.{FileExtension}");

        using var reader = new StreamReader(fileStream);

        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<RulingsExcelMap>();

        var startTime = DateTime.Now;

        _logger.LogInformation("Starting Database Update Process at: {timeNow}", startTime);

        _rulings = csv.GetRecords<Ruling>().ToList();

        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        await context.BulkInsertAllAsync(_rulings, cancellationToken);

        _rulings.Clear();

        _rulings = null;

        _logger.LogInformation("Finished Database Update Process in: {timeNow} seconds", (DateTime.Now - startTime).TotalSeconds);
    }

    private async Task ClearRulings()
    {
        _logger.LogInformation("Cleaning Previous Rulings from Database at {timeStarted}", DateTime.Now);

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var cmd = $"TRUNCATE TABLE {AnnotationHelper.TableName(context.Rulings)}";

        await context.Database.ExecuteSqlRawAsync(cmd);

        await context.SaveChangesAsync();

        _logger.LogInformation("Cleared Previous Rulings from Database at {timeEnded}", DateTime.Now);
    }
}