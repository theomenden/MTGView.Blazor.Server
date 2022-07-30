using MTGView.Data.Background.Internal;
using MTGView.Data.EFCore.Extensions;
using Polly;
using Polly.Extensions.Http;
namespace MTGView.Data.Background.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackgroundProcessingServicesForBlazor(this IServiceCollection services, String mtgApiConnectionString)
    {
        services.AddHttpClient("MtgJsonClient", client =>
            {
                client.BaseAddress = new(mtgApiConnectionString);
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services
            .AddScoped<IUnzippingService, FileProcessingService>()
            .AddScoped<IReplaceKeywordsService, ReplaceKeywordsService>()
            .AddScoped<IReplaceCardsService, ReplaceCardsService>()
            .AddScoped<IReplaceRulingsService, ReplaceRulingsService>()
            .AddScoped<IReplaceLegalitiesService, ReplaceLegalitiesService>()
            .AddScoped<IReplaceSetsService, ReplaceSetsService>();

        return services;
    }

    public static IServiceCollection AddBackgroundProcessingServices(this IServiceCollection services, IDictionary<String, String> connectionStrings)
    {
        services.AddHttpClient("MtgJsonClient", client =>
        {
            client.BaseAddress = new(connectionStrings["MtgApi"]);
        })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddMtgDataServices(connectionStrings["MtgDb"]);

        services.AddScoped<IUnzippingService, FileProcessingService>()
                .AddScoped<IReplaceKeywordsService, ReplaceKeywordsService>()
                .AddScoped<IReplaceCardsService, ReplaceCardsService>()
                .AddScoped<IReplaceRulingsService, ReplaceRulingsService>()
                .AddScoped<IReplaceLegalitiesService, ReplaceLegalitiesService>()
                .AddScoped<IReplaceSetsService, ReplaceSetsService>();

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}

