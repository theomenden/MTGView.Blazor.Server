using System.Text.Json.Serialization;

namespace MTGView.Scryfall.Core.Models;

/// <summary>
/// Class describing the structure of the Scryfall Symbology API Endpoint
/// </summary>
public sealed class Symbology
{
    [JsonPropertyName("_object")]
    public string Object { get; set; }

    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }

    [JsonPropertyName("data")] public IEnumerable<SymbologyDatum> Data { get; set; } = new List<SymbologyDatum>(200);
}


public sealed class SymbologyDatum
{
    [JsonPropertyName("_object")]
    public string Object { get; set; }

    /// <value>
    /// String Representation of the symbol - e.g. "Tap" -&gt; {T}
    /// </value>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }

    [JsonPropertyName("svg_uri")]
    public string SvgUri { get; set; }

    [JsonPropertyName("loose_variant")]
    public string LooseVariant { get; set; }

    [JsonPropertyName("english")]
    public string English { get; set; }

    [JsonPropertyName("transposable")]
    public bool IsTransposable { get; set; }

    [JsonPropertyName("represents_mana")]
    public bool IsManaRepresentative { get; set; }

    [JsonPropertyName("appears_in_mana_costs")]
    public bool CanBeInManaCost { get; set; }

    [JsonPropertyName("cmc")]
    public decimal? ConvertedManaCost { get; set; }

    [JsonPropertyName("funny")]
    public bool IsFunny { get; set; }

    [JsonPropertyName("colors")]
    public string[] Colors { get; set; }

    [JsonPropertyName("gatherer_alternates")]
    public string[] GathererAlternates { get; set; }
}
