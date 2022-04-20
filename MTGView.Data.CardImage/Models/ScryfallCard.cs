using System.Text.Json.Serialization;

namespace MTGView.Data.Scryfall.Models;

/// <summary>
/// Parent class for Scryfall Api information - retrives the entire object based off of the scryfall id
/// </summary>
public class ScryfallCard
{
    public string _object { get; set; }

    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    public string oracle_id { get; set; }
    public int[] multiverse_ids { get; set; }
    public int mtgo_id { get; set; }
    public int tcgplayer_id { get; set; }
    public int cardmarket_id { get; set; }
    public string name { get; set; }
    public string lang { get; set; }
    public string released_at { get; set; }
    public string uri { get; set; }
    public string scryfall_uri { get; set; }
    public string layout { get; set; }
    public bool highres_image { get; set; }
    public string image_status { get; set; }
    public ImageUris image_uris { get; set; }
    public string mana_cost { get; set; }
    public decimal cmc { get; set; }
    public string type_line { get; set; }
    public string[] colors { get; set; }
    public string[] color_identity { get; set; }
    public string[] keywords { get; set; }
    public CardFaces[] card_faces { get; set; }
    public Legalities legalities { get; set; }
    public string[] games { get; set; }
    public bool reserved { get; set; }
    public bool foil { get; set; }
    public bool nonfoil { get; set; }
    public string[] finishes { get; set; }
    public bool oversized { get; set; }
    public bool promo { get; set; }
    public bool reprint { get; set; }
    public bool variation { get; set; }

    [JsonPropertyName("set_id")]
    public string SetId { get; set; }
    public string set { get; set; }

    [JsonPropertyName("set_name")]
    public string SetName { get; set; }
    public string set_type { get; set; }

    [JsonPropertyName("set_uri")]
    public string SetUri { get; set; }
    public string set_search_uri { get; set; }
    public string scryfall_set_uri { get; set; }
    public string rulings_uri { get; set; }
    public string prints_search_uri { get; set; }
    public string collector_number { get; set; }
    public bool digital { get; set; }
    public string rarity { get; set; }
    public string card_back_id { get; set; }
    public string artist { get; set; }
    public string[] artist_ids { get; set; }
    public string illustration_id { get; set; }
    public string border_color { get; set; }
    public string frame { get; set; }
    public string security_stamp { get; set; }
    public bool full_art { get; set; }
    public bool textless { get; set; }
    public bool booster { get; set; }
    public bool story_spotlight { get; set; }
    public int edhrec_rank { get; set; }
    public Prices prices { get; set; }
    public RelatedUris related_uris { get; set; }
    public PurchaseUris purchase_uris { get; set; }
}
