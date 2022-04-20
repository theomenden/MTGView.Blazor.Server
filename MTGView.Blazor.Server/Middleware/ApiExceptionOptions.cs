namespace MTGView.Blazor.Server.Middleware;

public class ApiExceptionOptions
{
    public Action<HttpContext, Exception, OperationOutcome> AddResponseDetails { get; set; }

    public Func<Exception, LogLevel> DetermineLogLevel { get; set; }
}
