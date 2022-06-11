using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using MTGView.Core.Models;
using MTGView.Data.Background.Interfaces;

namespace MTGView.Data.Background.Internal;

internal sealed class DeserializeCardsService: IDeserializationService
{
    private const string FileName = "AllPrintings.json";

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        AllowTrailingCommas = true,
        ReferenceHandler = ReferenceHandler.Preserve
    };

    private readonly ILogger<DeserializeCardsService> _logger;

    public DeserializeCardsService(ILogger<DeserializeCardsService> logger)
    {
        _logger = logger;
    }

    public async IAsyncEnumerable<MagicCard> DeserializeFileToMagicCardsAsync(String filePath, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        await foreach (var magicCard in JsonSerializer.DeserializeAsyncEnumerable<MagicCard>(fileStream, _jsonSerializerOptions, cancellationToken))
        {
            yield return magicCard;
        }
    }
}