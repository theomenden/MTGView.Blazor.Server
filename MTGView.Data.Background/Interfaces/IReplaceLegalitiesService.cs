namespace MTGView.Data.Background.Interfaces;

public interface IReplaceLegalitiesService
{
    Task DeserializeCsvToLegalities(String fileName, CancellationToken cancellationToken = default);
}

