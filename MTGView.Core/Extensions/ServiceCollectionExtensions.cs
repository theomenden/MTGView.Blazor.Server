using System.Reflection;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MTGView.Core.Mapping;
using MTGView.Core.Models;

namespace MTGView.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAutomappingProfiles<T>(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(T).GetTypeInfo().Assembly, typeof(MagicCard).GetTypeInfo().Assembly);
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MtgMappingProfile());
        });

        var mapper = config.CreateMapper();

        services.AddSingleton(mapper);

        return services;
    }
}