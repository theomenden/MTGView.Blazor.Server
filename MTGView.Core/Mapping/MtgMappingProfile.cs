using AutoMapper;
using MTGView.Core.Models;

namespace MTGView.Core.Mapping;
public sealed class MtgMappingProfile: Profile
{
    public MtgMappingProfile()
    {
        CreateMap<MtgCardExcelData, MagicCard>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .ReverseMap();
    }
}

