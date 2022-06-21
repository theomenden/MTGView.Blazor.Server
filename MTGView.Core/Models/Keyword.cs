using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MTGView.Core.Models;

public class Keyword
{
    [JsonIgnore]
    [Key]
    public Int32 Id { get; set; }

    public string Name { get; set; }

    public string RecordType { get; set; }
}
