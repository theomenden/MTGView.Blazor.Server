namespace MTGView.Core.Logging;

/// <summary>
/// Templates for logging statements throughout the application
/// </summary>
public class MessageTemplates
{
    /// <value>
    /// Standard error log statement with correlation id
    /// </value>
    public const string DefaultLog = "{errorMessage} -- {correlationId}";

    /// <value>
    /// HttpClient logging statement for GET requests
    /// </value>
    public const string HttpClientGet = "GET request to {@absolutePath}";

    /// <value>
    /// Uncaught exception logging statement, with correlation id
    /// </value>
    public const string UncaughtGlobal = "Uncaught Exception. {resolvedExceptionMessage} -- {correlationId}";

    /// <value>
    /// Validation failure logging statement
    /// </value>
    public const string ValidationErrorsLog = "Validation Fail. {errors}. User Id {userName}.";
}