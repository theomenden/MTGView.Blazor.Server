using MTGView.Core.Models;

namespace MTGView.Data.Background.Interfaces;
public interface IDeserializationService
{
    IAsyncEnumerable<MagicCard> DeserializeFileToMagicCardsAsync(String filePath,
        CancellationToken cancellationToken = default);
}

