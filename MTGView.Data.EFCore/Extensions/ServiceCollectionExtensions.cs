namespace MTGView.Data.EFCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static ILoggerFactory _loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        public static IServiceCollection AddMtgDataServices(this IServiceCollection services, String connectionString)
        {
            services.AddPooledDbContextFactory<MagicthegatheringDbContext>(config =>
            {
                config.UseLoggerFactory(_loggerFactory)
                    .UseSqlServer(connectionString)
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
                    .EnableServiceProviderCaching();
            });

            return services;
        }

    }
}
