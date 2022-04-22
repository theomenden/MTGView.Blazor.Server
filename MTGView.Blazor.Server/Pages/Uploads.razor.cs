using System.Globalization;
using CsvHelper;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using MTGView.Blazor.Server.Models;
using MTGView.Core.Extensions;


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

        await SanitizeRecordsFromCsv(context, file);
    }

    private void OnWritten(FileWrittenEventArgs e)
    {
        Console.WriteLine($"File: {e.File.Name} Position: {e.Position} Data: {Convert.ToBase64String(e.Data)}");
    }

    private void OnProgressed(FileProgressedEventArgs e)
    {
        Console.WriteLine($"File: {e.File.Name} Progress: {e.Percentage}");
    }

    private async Task SanitizeRecordsFromCsv(MagicthegatheringDbContext context, IFileEntry csvToRead)
    {
        try
        {
            await using var stream = csvToRead.OpenReadStream(long.MaxValue);

            using var reader = new StreamReader(stream);

            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap<MagicCardExcelMap>();

            var deserializedMagicCardStream = csv.EnumerateRecordsAsync<MagicCard>(new ());

            await foreach (var magicCard in deserializedMagicCardStream)
            {
                context.Cards.Update(magicCard);

                await context.SaveChangesAsync();
            }

        }
        catch (Exception ex)
        {
            Logger.LogError("Exception while trying to parse {file}: {@ex}", csvToRead.Name, ex);
        }
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