using System.Text.Json.Serialization;
#nullable disable
namespace MTGView.Data.Scryfall.Models;
/// <summary>
/// Container for image information for Scryfall high resolution images
/// </summary>
public class ImageUris
{
    /// <value>
    /// Small, lowest resolution image available
    /// </value>
    [JsonPropertyName("small")]
    public string Small { get; set; }

    /// <value>
    /// Image contained within a normal view port
    /// </value>
    [JsonPropertyName("normal")]
    public string Normal { get; set; }

    /// <value>
    /// Image near max size (672px width)
    /// </value>
    [JsonPropertyName("large")]
    public string Large { get; set; }

    /// <value>
    /// The highest resolution of a card
    /// </value>
    [JsonPropertyName("png")]
    public string HighResolution { get; set; }

    /// <value>
    /// Card with only art
    /// </value>
    [JsonPropertyName("art_crop")]
    public string ArtCropped { get; set; }

    /// <value>
    /// Card with no border
    /// </value>
    [JsonPropertyName("border_crop")]
    public string BorderCropped { get; set; }

    /// <summary>
    /// Used for Responsive Lazy Loading via lazysizes.js
    /// </summary>
    /// <returns>image urls of various sizes</returns>
    public IEnumerable<String> GetAllImagesAsSizes()
    {
        return new []
        {
            $"{Normal} 488w",
            $"{Large} 672w",
            $"{HighResolution} 745w",
        };
    }
}