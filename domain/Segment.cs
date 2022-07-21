using System.Text.Json.Serialization;

namespace CWIllumigon.Domain;

public class Segment
{
    [JsonPropertyName("start")] public int Start { get; set; }
    [JsonPropertyName("stop")] public int Stop { get; set; }
    [JsonPropertyName("len")] public int Length { get; set; }
    [JsonPropertyName("on")] public bool IsOn { get; set; } = true;
    [JsonPropertyName("col")] public List<List<int>> Color { get; } = new();
}