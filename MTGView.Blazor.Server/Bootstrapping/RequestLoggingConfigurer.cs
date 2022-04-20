namespace MTGView.Blazor.Server.Bootstrapping;

public static class RequestLoggingConfigurer
{
    public static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext context)
    {
        var request = context.Request;

        diagnosticContext.Set("Host", request.Host);
        diagnosticContext.Set("Protocol", request.Protocol);
        diagnosticContext.Set("Scheme", request.Scheme);

        if (request.QueryString.HasValue)
        {
            diagnosticContext.Set("QueryString", request.QueryString.Value);
        }

        diagnosticContext.Set("ContentType", context.Response.ContentType);

        var endpoint = context.GetEndpoint();

        if (endpoint is not null)
        {
            diagnosticContext.Set("EndpointName", endpoint.DisplayName);
        }
    }
}
