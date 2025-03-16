using System.Text.Json.Serialization;

namespace TaterSharp.Common.ApiModels;

public class LastHashResponse
{
    [JsonPropertyName("hash")]
    public string Hash { get; set; } = string.Empty;

}