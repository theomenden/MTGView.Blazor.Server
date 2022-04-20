namespace MTGView.Core.Enums;

/// <summary>
/// Contains the various types of logging events for errors.
/// </summary>
public enum EventIdIdentifier
{
    /// <value>
    /// 
    /// </value>
    AppThrown = 1,
    /// <value>
    /// 
    /// </value>
    UncaughtInAction = 2,
    /// <value>
    /// 
    /// </value>
    UncaughtGlobal = 3,
    /// <value>
    /// 
    /// </value>
    PipelineThrown = 4,
    /// <value>
    /// 
    /// </value>
    HttpClient = 5
}