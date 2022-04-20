namespace MTGView.Data.Scryfall.Services;

public interface IScryfallSymbologyService
{
    Task<ApiResponse<IEnumerable<SymbologyDatum>>> GetAllSymbolsFromScryfall(CancellationToken cancellationToken = default);
}

