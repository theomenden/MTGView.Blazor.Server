namespace MTGView.Data.Background.Interfaces;

public interface IReplaceRulingsService
{
    Task DeserializeCsvToRulings(String fileName, CancellationToken cancellationToken = default);
}