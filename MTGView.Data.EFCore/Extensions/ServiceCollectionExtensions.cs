namespace MTGView.Data.EFCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        public static IServiceCollection AddMtgDataServices(this IServiceCollection services, String connectionString)
        {
            services.AddPooledDbContextFactory<MagicthegatheringDbContext>(config =>
            {
                config
                    .UseSqlServer(connectionString)
                    .UseLoggerFactory(LoggerFactory)
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
                    .EnableServiceProviderCaching();
            });

            return services;
        }

    }
}
