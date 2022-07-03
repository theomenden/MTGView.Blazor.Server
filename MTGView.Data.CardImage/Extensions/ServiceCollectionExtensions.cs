using Microsoft.Extensions.DependencyInjection;
using MTGView.Data.Scryfall.Internal;
using TheOmenDen.Shared.Extensions.DependencyInjection;

namespace MTGView.Data.Scryfall.Extensions;

public static class ServiceCollectionExtensions
{
    private const string ScryfallApiEndpoint = "https://api.scryfall.com";

    public static IServiceCollection AddScryfallApiServices(this IServiceCollection services)
    {
        services.AddTransient<ScryfallCardService>();
        services.AddTransient<ScryfallSymbologyService>();
        services.AddTransient<ScryfallSetInformationService>();

        services.AddTheOmenDenHttpServices<ScryfallCardService>(
            new HttpClientConfiguration
            {
                BaseAddress = ScryfallApiEndpoint,
                Name = "Scryfall"
            });

        return services;
    }
}
