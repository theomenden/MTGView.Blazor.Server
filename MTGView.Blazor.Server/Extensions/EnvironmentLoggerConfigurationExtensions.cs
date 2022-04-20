using MTGView.Core.Logging;
using Serilog.Configuration;

namespace MTGView.Blazor.Server.Extensions;

public static class EnvironmentLoggerConfigurationExtensions
{
    /// <summary>
    /// Enriches log events with a hash of the message template to uniquely identify different event types
    /// </summary>
    /// <param name="enrichmentConfiguration">The supplied enrichment configuration</param>
    /// <returns><see cref="LoggerConfiguration"/>. The logger configuration object resulting from the logger enrichment</returns>
    /// <exception cref="ArgumentNullException">If the supplied <paramref name="enrichmentConfiguration"/> is null</exception>
    public static LoggerConfiguration WithEventType(this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        if (enrichmentConfiguration is null)
        {
            throw new ArgumentNullException(nameof(enrichmentConfiguration));
        }

        return enrichmentConfiguration.With<EventTypeEnricher>();
    }
}
