namespace MTGView.Data.EFCore.Extensions;

public static class ServiceCollectionExtensions
{
#if DEBUG
    private static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
    {
        builder.AddJsonConsole();
        builder.AddConsole();
    });
#endif
    public static IServiceCollection AddMtgDataServices(this IServiceCollection services, String connectionString)
    {
        services.AddPooledDbContextFactory<MagicthegatheringDbContext>(config =>
        {
            config
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
