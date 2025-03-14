using System.Text.Json.Serialization;

namespace TaterSharp.CLI.ApiModels;

public class LastHashResponse
{
    [JsonPropertyName("hash")]
    public string Hash { get; set; } = string.Empty;

}