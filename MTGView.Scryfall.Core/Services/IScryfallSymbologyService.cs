namespace MTGView.Scryfall.Core.Services;

public interface IScryfallSymbologyService
{
    Task<ApiResponse<IEnumerable<SymbologyDatum>>> GetAllSymbolsFromScryfall(CancellationToken cancellationToken = default);
}

