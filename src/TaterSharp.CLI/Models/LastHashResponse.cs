using System.Text.Json.Serialization;

namespace TaterSharp.CLI.Models;

public class LastHashResponse
{
    [JsonPropertyName("hash")]
    public string Hash { get; set; }

}