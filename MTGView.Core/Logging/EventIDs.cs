using Microsoft.Extensions.Logging;
using MTGView.Core.Enums;

namespace MTGView.Core.Logging;

public static class EventIDs
{
    public static readonly EventId EventIdAppThrown = new((int)EventIdIdentifier.AppThrown, EventIdIdentifier.AppThrown.ToString());

    public static readonly EventId EventIdHttpClient = new((int)EventIdIdentifier.HttpClient, EventIdIdentifier.HttpClient.ToString());

    public static readonly EventId EventIdPipelineThrown = new((int)EventIdIdentifier.PipelineThrown, EventIdIdentifier.PipelineThrown.ToString());

    public static readonly EventId EventIdUncaught = new((int)EventIdIdentifier.UncaughtInAction, EventIdIdentifier.UncaughtInAction.ToString());

    public static readonly EventId EventIdUncaughtGlobal = new((int)EventIdIdentifier.UncaughtGlobal, EventIdIdentifier.UncaughtGlobal.ToString());
}