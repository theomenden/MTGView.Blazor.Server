using MTGView.Data.Background;

namespace MTGView.Blazor.Server.Models;

/// <summary>
/// Record to simplify the enabling/disabling of <see cref="BackgroundUpdatingService"/>
/// </summary>
/// <param name="IsEnabled">Sets the state of the service</param>
public record BackgroundUpdatingServiceState(Boolean IsEnabled);
