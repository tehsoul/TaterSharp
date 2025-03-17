using System.Text.Json.Serialization;

namespace TaterSharp.Common.ApiModels;
public class SystemTimeResponse
{
    [JsonPropertyName("system_time")]
    public long SystemTime { get; set; }
}
