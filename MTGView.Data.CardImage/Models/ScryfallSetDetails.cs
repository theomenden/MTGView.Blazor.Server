using System.Text.Json.Serialization;

namespace MTGView.Data.Scryfall.Models;

/// <summary>
/// Container class describing the structure returned from the Scryfall Set API Endpoint
/// </summary>


public class ScryfallSetRootInformation
{
    [JsonPropertyName("_object")]
    public string _object { get; set; }

    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }

    [JsonPropertyName("data")]
    public ScryfallSetDetails[] Data { get; set; }
}


public class ScryfallSetDetails
{
    [JsonPropertyName("_object")]
    public string Object { get; set; }

    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("mtgo_code")]
    public string MTGOCode { get; set; }

    [JsonPropertyName("arena_code")]
    public string ArenaCode { get; set; }

    [JsonPropertyName("tcgplayer_id")]
    public int TCGPlayerId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("uri")]
    public string Uri { get; set; }

    [JsonPropertyName("scryfall_uri")]
    public string ScryfallUri { get; set; }

    [JsonPropertyName("search_uri")]
    public string SearchUri { get; set; }

    [JsonPropertyName("released_at")]
    public DateTime ReleasedAt { get; set; }

    [JsonPropertyName("set_type")]
    public string SetType { get; set; }

    [JsonPropertyName("card_count")]
    public int CardsInSet { get; set; }

    [JsonPropertyName("printed_size")]
    public int PrintedSize { get; set; }

    [JsonPropertyName("digital")]
    public bool IsDigital { get; set; }

    [JsonPropertyName("nonfoil_only")]
    public bool IsNonFoilOnly { get; set; }

    [JsonPropertyName("foil_only")]
    public bool IsFoilOnly { get; set; }

    [JsonPropertyName("block_code")]
    public string BlockCode { get; set; }

    [JsonPropertyName("block")]
    public string Block { get; set; }

    [JsonPropertyName("icon_svg_uri")]
    public string IconUri { get; set; }
}

