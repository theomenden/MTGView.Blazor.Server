using MTGView.Core.Models;

namespace MTGView.Data.Background.Interfaces;
public interface IReplaceCardsService
{
    Task DeserializeCsvToMagicCards(String fileName, CancellationToken cancellationToken = default);
}

