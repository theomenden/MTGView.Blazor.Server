namespace MTGView.Data.Scryfall.Models;
public class ImageUris
{
    public string small { get; set; }
    public string normal { get; set; }
    public string large { get; set; }
    public string png { get; set; }
    public string art_crop { get; set; }
    public string border_crop { get; set; }

    public IEnumerable<String> GetAllImagesAsSizes()
    {
        return new []
        {
            $"{normal} 488w",
            $"{large} 672w",
            $"{png} 745w",
        };
    }
}