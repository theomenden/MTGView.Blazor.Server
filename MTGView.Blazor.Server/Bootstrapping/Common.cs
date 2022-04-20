using System.Text.Json;
using System.Text.Json.Serialization;

namespace MTGView.Blazor.Server.Bootstrapping;
public class Common
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
