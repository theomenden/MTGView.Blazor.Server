using CsvHelper.Configuration;
using MTGView.Core.Models;

namespace MTGView.Core.Mapping.ExcelMappings;

public sealed class RulingsExcelMap : ClassMap<Ruling>
{
    public RulingsExcelMap()
    {
        Map(ruling => ruling.Index).Name("index");
        Map(ruling => ruling.Id).Name("id");
        Map(ruling => ruling.CreatedAt).Name("date");
        Map(ruling => ruling.RuleText).Name("text");
        Map(ruling => ruling.RulingGuid).Name("uuid");
    }

    private static int ConvertFromStringToInteger(string valueToConvert) => int.TryParse(valueToConvert, out var result) ? result : 0;
}
