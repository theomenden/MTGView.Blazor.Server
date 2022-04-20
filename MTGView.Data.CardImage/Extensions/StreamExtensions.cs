#nullable disable
namespace MTGView.Data.Scryfall.Extensions;
public static class StreamExtensions
{
    public static async Task<T> DeserializeFromStreamAsync<T>(this Stream stream, CancellationToken cancellationToken = default)
    {
        if (stream is null || stream.CanRead is false)
        {
            return default;
        }

        var searchResult = await JsonSerializer.DeserializeAsync<T>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

        return searchResult;
    }

    public static async Task<String> DeserializeStreamToStringAsync(this Stream stream)
    {
        var content = String.Empty;

        if (stream is null)
        {
            return content;
        }

        using var sr = new StreamReader(stream);

        content = await sr.ReadToEndAsync();

        return content;
    }
}