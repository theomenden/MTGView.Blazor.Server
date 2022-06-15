using CsvHelper.Configuration;
using MTGView.Core.Models;

namespace MTGView.Core.Mapping.ExcelMappings;
public sealed class LegalityExcelMap: ClassMap<Legality>
{
    public LegalityExcelMap()
    {
        Map(legality => legality.index).Name("index");
        Map(legality => legality.id).Name("id");
        Map(legality => legality.format).Name("format");
        Map(legality => legality.status).Name("status");
        Map(legality => legality.uuid).Name("uuid");
    }
}
