using System.Runtime.CompilerServices;

namespace MTGView.Data.Background.Interfaces;
public interface IReplaceKeywordsService
{
    Task DownloadKeywordsData(CancellationToken cancellationToken = default);
}
