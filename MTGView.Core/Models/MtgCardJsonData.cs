#nullable disable
using System.Text.Json.Serialization;

namespace MTGView.Core.Models;

public class MtgCardJsonData
{
    [JsonPropertyName("meta")]
    public Meta Meta { get; set; }

    [JsonPropertyName("data")]
    public IEnumerable<MagicSet> Data { get; set; }
}
