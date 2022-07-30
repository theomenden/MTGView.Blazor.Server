namespace MTGView.Data.Background.Interfaces;
/// <summary>
/// Defines methods for deserializing and manipulating sets within a context.
/// </summary>
public interface IReplaceSetsService
{
    Task DeserializeCsvToSets(CancellationToken cancellationToken = default);
}