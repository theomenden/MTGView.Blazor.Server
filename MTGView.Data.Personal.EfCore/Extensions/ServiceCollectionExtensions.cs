using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MTGView.Data.Personal.EfCore.Contexts;

namespace MTGView.Data.Personal.EfCore.Extensions;

public static class ServiceCollectionExtensions
{
#if DEBUG
    private static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    });
#endif
    public static IServiceCollection AddPersonalCollectionServices(this IServiceCollection services, String connectionString)
    {
        services.AddPooledDbContextFactory<PersonalcollectionsDbContext>(options =>
        {
            options
                .UseSqlServer(connectionString)
#if DEBUG
                .UseLoggerFactory(LoggerFactory)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
#endif
                .EnableServiceProviderCaching();
        });

        return services;
    }
}