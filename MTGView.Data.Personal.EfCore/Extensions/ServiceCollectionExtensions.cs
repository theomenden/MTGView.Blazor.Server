using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MTGView.Data.Personal.EfCore.Contexts;

namespace MTGView.Data.Personal.EfCore.Extensions;

public static class ServiceCollectionExtensions
{
    private static ILoggerFactory _loggerFactory = LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    });

    public static IServiceCollection AddPersonalCollectionServices(this IServiceCollection services, String connectionString)
    {
        services.AddPooledDbContextFactory<PersonalcollectionsDbContext>(options =>
        {
            options.UseLoggerFactory(_loggerFactory)
                .UseSqlServer(connectionString)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .EnableServiceProviderCaching();
        });

        return services;
    }
}