namespace MTGView.Scryfall.Core.Services;

public interface IScryfallCardService
{
    /// <summary>
    /// Retrieves the entire Card object from Scryfall
    /// </summary>
    /// <param name="scryfallId">The id that we have from MTG Json</param>
    /// <param name="cancellationToken"></param>
    /// <returns><see cref="ApiResponse"/>: <seealso cref="ScryfallCard"/></returns>
    Task<ApiResponse<ScryfallCard>> GetScryfallInformationAsync(Guid scryfallId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the Card objects from scryfall for each referenced <paramref name="scryfallIds"/>
    /// </summary>
    /// <param name="scryfallIds">The ids of each card we want to get information for</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<ApiResponse<ScryfallCard>> GetScryfallInformationAsync(IEnumerable<Guid> scryfallIds, CancellationToken cancellationToken = default);
}

