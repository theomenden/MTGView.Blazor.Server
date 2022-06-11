﻿using MTGView.Data.Background.Interfaces;
using MTGView.Data.Background.Internal;
using MTGView.Data.EFCore.Extensions;
using MTGView.Core.Extensions;
using Polly;
using Polly.Extensions.Http;
namespace MTGView.Data.Background.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackgroundProcessingServices(this IServiceCollection services, IDictionary<String, String> connectionStrings)
    {
        services.AddHttpClient("MtgJsonClient",client =>
        {
            client.BaseAddress = new (connectionStrings["MtgApi"]);
        })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy()); ;

        services.AddMtgDataServices(connectionStrings["MtgDb"]);

        services.AddScoped<IUnzippingService, UnzippingService>();
        services.AddScoped<IDeserializationService, DeserializeCardsService>();
        
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

