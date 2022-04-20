using MTGView.Blazor.Server.Middleware;

namespace MTGView.Blazor.Server.Extensions;

public static class ExceptionLoggerExtensions
{
    public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder builder)
    {
        var options = new ApiExceptionOptions();

        return builder.UseMiddleware<ExceptionLogger>(options);
    }

    public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder builder,
        Action<ApiExceptionOptions> configureOptions)
    {
        var options = new ApiExceptionOptions();
        configureOptions(options);

        return builder.UseMiddleware<ExceptionLogger>(options);
    }
}