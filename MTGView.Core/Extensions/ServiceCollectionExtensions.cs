using Microsoft.Extensions.DependencyInjection;

namespace MTGView.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAutomappingProfiles<T>(this IServiceCollection services)
    {
        return services;
    }
}