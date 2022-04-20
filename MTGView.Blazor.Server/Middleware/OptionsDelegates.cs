using Microsoft.Data.SqlClient;

namespace MTGView.Blazor.Server.Middleware;

public static class OptionsDelegates
{
    public static void UpdateApiErrorResponse(HttpContext context, Exception exception,
        OperationOutcome operationOutcome)
    {
        if (exception.GetType().Name.Equals(nameof(SqlException), StringComparison.OrdinalIgnoreCase))
        {
            operationOutcome.Message += "The exception was a database exception";
        }
    }

    public static LogLevel DetermineLogLevel(Exception exception) =>
        exception.Message.StartsWith("cannot open database", StringComparison.OrdinalIgnoreCase) ||
        exception.Message.StartsWith("a network-related", StringComparison.OrdinalIgnoreCase)
            ? LogLevel.Critical
            : LogLevel.Error;
}
