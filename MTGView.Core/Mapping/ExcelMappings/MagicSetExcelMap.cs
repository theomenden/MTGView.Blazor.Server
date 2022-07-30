using CsvHelper.Configuration;
using MTGView.Core.Models;

namespace MTGView.Core.Mapping.ExcelMappings;
public sealed class MagicSetExcelMap: ClassMap<MagicSet>
{
    public MagicSetExcelMap()
    {
        Map(magicSet => magicSet.id).Name("id");
        Map(magicSet => magicSet.index).Name("index");
        Map(magicSet => magicSet.baseSetSize).Name("baseSetSize");
        Map(magicSet => magicSet.block).Name("block");
        Map(magicSet => magicSet.booster).Name("booster");
        Map(magicSet => magicSet.code).Name("code");
        Map(magicSet => magicSet.isFoilOnly).Name("isFoilOnly");
        Map(magicSet => magicSet.isForeignOnly).Name("isForeignOnly");
        Map(magicSet => magicSet.isNonFoilOnly).Name("isNonFoilOnly");
        Map(magicSet => magicSet.isOnlineOnly).Name("isOnlineOnly");
        Map(magicSet => magicSet.isPartialPreview).Name("isPartialPreview");
        Map(magicSet => magicSet.keyruneCode).Name("keyruneCode");
        Map(magicSet => magicSet.mcmId).Name("mcmId");
        Map(magicSet => magicSet.mcmIdExtras).Name("mcmIdExtras");
        Map(magicSet => magicSet.mcmName).Name("mcmName");
        Map(magicSet => magicSet.mtgoCode).Name("mtgoCode");
        Map(magicSet => magicSet.name).Name("name");
        Map(magicSet => magicSet.parentCode).Name("parentCode");
        Map(magicSet => magicSet.releaseDate).Name("releaseDate");
        Map(magicSet => magicSet.sealedProduct).Name("sealedProduct");
        Map(magicSet => magicSet.tcgplayerGroupId).Name("tcgplayerGroupId");
        Map(magicSet => magicSet.totalSetSize).Name("totalSetSize");
        Map(magicSet => magicSet.type).Name("type");
    }
}