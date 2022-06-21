using System.Text.Json.Serialization;
#nullable disable
namespace MTGView.Core.Models;

public class Keywords
{
    [JsonPropertyName("abilityWords")]
    public IEnumerable<String> AbilityWords { get; set; }
    
    [JsonPropertyName("keywordAbilities")]
    public IEnumerable<String> KeywordAbilities { get; set; }
    
    [JsonPropertyName("keywordActions")]
    public IEnumerable<String> KeywordActions { get; set; }
}

public class RootKeywordsData
{
    [JsonPropertyName("meta")]
    public Meta Meta { get; set; }

    [JsonPropertyName("data")]
    public Keywords Data { get; set; }
}
