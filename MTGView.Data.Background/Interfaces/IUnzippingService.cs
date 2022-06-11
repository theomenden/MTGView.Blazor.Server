namespace MTGView.Data.Background.Interfaces;

public interface IUnzippingService
{
    Task InitiateFileDownloadProcess(CancellationToken cancellationToken = default);
}
