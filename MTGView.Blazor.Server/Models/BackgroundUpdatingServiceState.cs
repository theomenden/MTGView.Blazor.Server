using MTGView.Data.Background;

namespace MTGView.Blazor.Server.Models;

/// <summary>
/// Record to simplify the enabling/disabling of <see cref="BackgroundUpdatingService"/>
/// </summary>
public record BackgroundUpdatingServiceState(Boolean IsEnabled, Boolean IsRunning);