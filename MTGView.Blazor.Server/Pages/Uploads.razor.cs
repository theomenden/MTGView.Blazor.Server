using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;

namespace MTGView.Blazor.Server.Pages;

public partial class Uploads : ComponentBase
{
    [Inject] public IDbContextFactory<MagicthegatheringDbContext> DbContextFactory { get; init; }

    [Inject] public IMapper Mapper { get; init; }

    [Inject] public ILogger<Uploads> Logger { get; init; }

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        AllowTrailingCommas = true,
        DefaultBufferSize = 1024
    };

    private async Task OnChanged(FileChangedEventArgs e)
    {
        try
        {
            var file = e.Files.FirstOrDefault();

            if (file is not null && file.Type.Equals("text/csv", StringComparison.OrdinalIgnoreCase))
            {
                await LoadCsvIntoDatabase(file);
            }
        }
        catch (Exception exception)
        {
            Logger.LogError("Exception thrown while attempting to read the {@file} provided {@ex}", e, exception);
        }
    }

    private async Task LoadCsvIntoDatabase(IFileEntry file)
    {
        await using var context = await DbContextFactory.CreateDbContextAsync();
        
        var magicCardsToUpdate = await SanitizeRecordsFromCsv(file);

        context.Cards.UpdateRange(magicCardsToUpdate);

        await context.SaveChangesAsync();
    }

    private void OnWritten(FileWrittenEventArgs e)
    {
        Console.WriteLine($"File: {e.File.Name} Position: {e.Position} Data: {Convert.ToBase64String(e.Data)}");
    }

    private void OnProgressed(FileProgressedEventArgs e)
    {
        Console.WriteLine($"File: {e.File.Name} Progress: {e.Percentage}");
    }

    private async Task<List<MagicCard>> SanitizeRecordsFromCsv(IFileEntry csvToRead)
    {
        var records = new List<MtgCardExcelData>(70_000);

        try
        {
            await using var stream = csvToRead.OpenReadStream(long.MaxValue);

            using var reader = new StreamReader(stream);

            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);


            await csv.ReadAsync();
            csv.ReadHeader();
            while (await csv.ReadAsync())
            {
                var record = new MtgCardExcelData
                {
                    index = csv.GetField<Int32>("index"),
                    id = csv.GetField<Int32>("id"),
                    artist = csv.GetField("artist"),
                    asciiName = csv.GetField("asciiName"),
                    availability = csv.GetField("availability"),
                    borderColor = csv.GetField("borderColor"),
                    cardParts = csv.GetField("cardParts"),
                    colorIdentity = csv.GetField("colorIdentity"),
                    colorIndicator = csv.GetField("colorIndicator"),
                    colors = csv.GetField("colors"),
                    duelDeck = csv.GetField("duelDeck"),
                    edhrecRank = Convert.ToInt16(csv.GetField("edhrecRank")),
                    faceFlavorName = csv.GetField("faceFlavorName"),
                    finishes = csv.GetField("finishes"),
                    flavorText = csv.GetField("flavorText"),
                    frameEffects = csv.GetField("frameEffects"),
                    frameVersion = csv.GetField("frameVersion"),
                    hasAlternativeDeckLimit = Convert.ToInt16(csv.GetField("hasAlternativeDeckLimit")),
                    hasContentWarning = Convert.ToInt16(csv.GetField("hasContentWarning")),
                    isAlternative = Convert.ToInt16(csv.GetField("isAlternative")),
                    isFullArt = Convert.ToInt16(csv.GetField("isFullArt")),
                    isFunny = Convert.ToInt16(csv.GetField("isFunny")),
                    isOnlineOnly = Convert.ToInt16(csv.GetField("isOnlineOnly")),
                    isOversized = Convert.ToInt16(csv.GetField("isOversized")),
                    isPromo = Convert.ToInt16(csv.GetField("isPromo")),
                    isRebalanced = Convert.ToInt16(csv.GetField("isRebalanced")),
                    isReprint = Convert.ToInt16(csv.GetField("isReprint")),
                    isReserved = Convert.ToInt16(csv.GetField("isReserved")),
                    isStarter = Convert.ToInt16(csv.GetField("isStarter")),
                    isStorySpotlight = Convert.ToInt16(csv.GetField("isStorySpotlight")),
                    isTextless = Convert.ToInt16(csv.GetField("isTextless")),
                    isTimeshifted = Convert.ToInt16(csv.GetField("isTimeshifted")),
                    keywords = csv.GetField("keywords"),
                    layout = csv.GetField("frameVersion"),
                    life = Convert.ToInt16(csv.GetField("life")),
                    loyalty = csv.GetField("loyalty"),
                    manaCost = csv.GetField("manaCost"),
                    manaValue = Convert.ToInt16(csv.GetField("manaValue")),
                    mtgjsonV4Id = csv.GetField("mtgjsonV4Id"),
                    mtgoFoilId = csv.GetField<Int32>("mtgoFoilId"),
                    mtgoId = csv.GetField<Int32>("mtgoId"),
                    multiverseId = csv.GetField<Int32>("multiverseId"),
                    name = csv.GetField("name"),
                    number = csv.GetField("number"),
                    originalPrintings = csv.GetField("originalPrintings"),
                    originalReleaseDate = csv.GetField("originalReleaseDate"),
                    otherFaceIds = csv.GetField("otherFaceIds"),
                    power = csv.GetField("power"),
                    printings = csv.GetField("printings"),
                    promoTypes = csv.GetField("promoTypes"),
                    purchaseUrls = csv.GetField("purchaseUrls"),
                    rarity = csv.GetField("rarity"),
                    rebalancedPrintings = csv.GetField("rebalancedPrintings"),
                    scryfallId = csv.GetField<Guid>("scryfallId"),
                    scryfallIllustrationId = csv.GetField<Guid>("scryfallIllustrationId"),
                    scryfallOracleId = csv.GetField<Guid>("scrfallOracleId"),
                    securityStamp = csv.GetField("securityStamp"),
                    setCode = csv.GetField("setCode"),
                    side = csv.GetField("side"),
                    signature = csv.GetField("signature"),
                    subtypes = csv.GetField("subtypes"),
                    supertypes = csv.GetField("supertypes"),
                    text = csv.GetField("text"),
                    toughness = csv.GetField("toughness"),
                    type = csv.GetField("type"),
                    types = csv.GetField("types"),
                    uuid = csv.GetField<Guid>("uuid"),
                    variations = csv.GetField("variations"),
                    watermark = csv.GetField("watermark")
                };
                records.Add(record);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError("Exception while trying to parse {file}: {@ex}", csvToRead.Name, ex);
        }
        return records.Select(record => Mapper.Map<MagicCard>(record)).ToList();
    }

    private static async Task<T> DeserializeFromStream<T>(Stream stream, CancellationToken cancellationToken = default)
    {
        if (stream?.CanRead == false)
        {
            return default;
        }

        var searchResult = await JsonSerializer.DeserializeAsync<T>(stream!, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }, cancellationToken);

        return searchResult;
    }

    private static async Task<String> DeserializeStreamToStringAsync(Stream stream)
    {
        var content = String.Empty;

        if (stream is null)
        {
            return content;
        }

        using var sr = new StreamReader(stream);

        content = await sr.ReadToEndAsync();

        return content;
    }
}