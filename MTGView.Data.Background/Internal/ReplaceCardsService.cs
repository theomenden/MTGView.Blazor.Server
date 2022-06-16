using System.Globalization;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using MTGView.Core.Mapping.ExcelMappings;
using MTGView.Core.Models;
using MTGView.Data.Background.Helpers;
using MTGView.Data.Background.Interfaces;
using MTGView.Data.EFCore.Contexts;

namespace MTGView.Data.Background.Internal;

internal sealed class ReplaceCardsService : IReplaceCardsService
{
    private readonly IDbContextFactory<MagicthegatheringDbContext> _dbContextFactory;

    private readonly ILogger<ReplaceCardsService> _logger;

    private List<MagicCard>? _magicCards = new(70_000);

    private const string FileExtension = "csv";

    public ReplaceCardsService(IDbContextFactory<MagicthegatheringDbContext> dbContextFactory,
        ILogger<ReplaceCardsService> logger)
    {
        _logger = logger;
        _dbContextFactory = dbContextFactory;
    }

    public async Task DeserializeCsvToMagicCards(String fileName, CancellationToken cancellationToken = default)
    {
        await ClearCards();

        await using var fileStream = File.OpenRead($"{fileName}.{FileExtension}");

        using var reader = new StreamReader(fileStream);

        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<MagicCardExcelMap>();

        var startTime = DateTime.Now;

        _magicCards = csv.GetRecords<MagicCard>().ToList();

        _logger.LogInformation("Starting Database Update Process at: {timeNow}", startTime);

        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        await context.BulkInsertAllAsync(_magicCards, cancellationToken);

        _logger.LogInformation("Finished Database Update Process in: {timeNow} seconds", (DateTime.Now - startTime).TotalSeconds);

        _magicCards.Clear();
        _magicCards = null;
    }

    private async Task ClearCards()
    {
        _logger.LogInformation("Cleaning Previous cards from Database at {timeStarted}", DateTime.Now);

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var cmd = $"TRUNCATE TABLE {AnnotationHelper.TableName(context.Cards)}";

        await context.Database.ExecuteSqlRawAsync(cmd);

        await context.SaveChangesAsync();

        _logger.LogInformation("Cleared Previous cards from Database at {timeEnded}", DateTime.Now);
    }
}