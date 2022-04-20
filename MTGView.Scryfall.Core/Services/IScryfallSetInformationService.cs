using MTGView.Scryfall.Core.Models;

namespace MTGView.Scryfall.Core.Services;

public interface IScryfallSetInformationService
{
    /// <summary>
    /// Retrieves all the sets from the scryfall Set API Endpoint
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns><see cref="ApiResponse{T}"/>: <seealso cref="IEnumerable{T}"/>: <seealso cref="ScryfallSetDetails"/></returns>
    Task<ApiResponse<IEnumerable<ScryfallSetDetails>>> GetAllSetsAsync(CancellationToken cancellationToken = default);
}