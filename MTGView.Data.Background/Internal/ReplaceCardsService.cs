using System.Globalization;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using MTGView.Core.Models;
using MTGView.Data.Background.Extensions;
using MTGView.Data.Background.Interfaces;
using MTGView.Data.Background.Models;
using MTGView.Data.EFCore.Contexts;

namespace MTGView.Data.Background.Internal;

internal sealed class ReplaceCardsService : IReplaceCardsService
{
    private readonly IDbContextFactory<MagicthegatheringDbContext> _dbContextFactory;

    private readonly ILogger<ReplaceCardsService> _logger;

    private const string FileExtension = "csv";

    private static readonly MagicCard CardReference = new();

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

        var cardsToAdd = (await csv.GetRecordsAsync<MagicCard>(cancellationToken).ToArrayAsync(cancellationToken)).Chunk(1000);
        
        foreach (var card in cardsToAdd)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

            context.ChangeTracker.AutoDetectChangesEnabled = false;

            await context.Cards.AddRangeAsync(card, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }
        
        _logger.LogInformation("Finished Database Update Process in: {timeNow} seconds", (DateTime.Now - startTime).TotalSeconds);
    }

    private Task ClearDatabase()
    {
        _logger.LogInformation("Cleaning Previous cards from Database at {timeStarted}", DateTime.Now);

        using var context = _dbContextFactory.CreateDbContext();

        var cmd = $"TRUNCATE TABLE {AnnotationHelper.TableName(context.Cards)}";

        context.Database.ExecuteSqlRaw(cmd);

        context.SaveChanges();

        _logger.LogInformation("Cleared Previous cards from Database at {timeEnded}", DateTime.Now);

        return Task.CompletedTask;
    }
}