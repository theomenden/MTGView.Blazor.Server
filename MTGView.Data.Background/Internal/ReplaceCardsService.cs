using System.Globalization;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using MTGView.Core.Models;
using MTGView.Data.Background.Helpers;
using MTGView.Data.Background.Interfaces;
using MTGView.Data.Background.Models;
using MTGView.Data.EFCore.Contexts;

namespace MTGView.Data.Background.Internal;

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
        await ClearDatabase();

        await using var fileStream = new FileStream($"{fileName}.{FileExtension}", FileMode.Open, FileAccess.Read);

        using var reader = new StreamReader(fileStream);

        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<MagicCardExcelMap>();

        var startTime = DateTime.Now;

        _logger.LogInformation("Starting Database Update Process at: {timeNow}", startTime);

        var cardsToAdd = new List<MagicCard>(70_000);

        cardsToAdd.AddRange(csv.GetRecords<MagicCard>());
        
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        await context.BulkInsertAllAsync(cardsToAdd, cancellationToken);

        _logger.LogInformation("Finished Database Update Process in: {timeNow} seconds", (DateTime.Now - startTime).TotalSeconds);
    }

    private async Task ClearDatabase()
    {
        _logger.LogInformation("Cleaning Previous cards from Database at {timeStarted}", DateTime.Now);

        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var cmd = $"TRUNCATE TABLE {AnnotationHelper.TableName(context.Cards)}";

        await context.Database.ExecuteSqlRawAsync(cmd);

        await context.SaveChangesAsync();

        _logger.LogInformation("Cleared Previous cards from Database at {timeEnded}", DateTime.Now);
    }
}