using Microsoft.Extensions.DependencyInjection;
using MTGView.Data.Scryfall.Internal;
using Polly;
using Polly.Extensions.Http;

namespace MTGView.Data.Scryfall.Extensions;

public static class ServiceCollectionExtensions
{
    private const string ScryfallApiEndpoint = "https://api.scryfall.com";

    private const string SetsEndpoint = $"{ScryfallApiEndpoint}/sets/";

    private const string SymbologyEndpoint = $"{ScryfallApiEndpoint}/symbology/";

    private const string CardsEndPoint = $"{ScryfallApiEndpoint}/cards/";

    public static IServiceCollection AddScryfallApiServices(this IServiceCollection services)
    {

        services.AddHttpClient<IScryfallSetInformationService, ScryfallSetInformationService>("scryfallSetContext", cfg =>
            {
                cfg.BaseAddress = new(SetsEndpoint);
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddHttpClient<IScryfallSymbologyService, ScryfallSymbologyService>("scryfallSymbologyContext", cfg =>
            {
                cfg.BaseAddress = new(SymbologyEndpoint);
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddHttpClient<IScryfallCardService, ScryfallCardService>("scryfallCardContext", cfg =>
        {
            cfg.BaseAddress = new (CardsEndPoint);
        })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddScoped<IScryfallCardService, ScryfallCardService>()
        .AddScoped<IScryfallSetInformationService, ScryfallSetInformationService>()
        .AddScoped<IScryfallSymbologyService, ScryfallSymbologyService>();

        return services;
    }


    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                retryAttempt)));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}
