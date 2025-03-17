using System.Text.Json.Serialization;

namespace TaterSharp.Common.ApiModels;
public class BlockInfoResponse
{
    [JsonPropertyName("block_id")]
    public long BlockId { get; set; }

    [JsonPropertyName("color")] 
    public string Color { get; set; } = string.Empty;

    [JsonPropertyName("era")]
    public string Era { get; set; } = string.Empty;

    [JsonPropertyName("hash")]
    public string Hash { get; set; } = string.Empty;

    [JsonPropertyName("miner_id")]
    public string MinerId { get; set; } = string.Empty;

    [JsonPropertyName("online")]
    public List<string> Online { get; set; } = [];

    [JsonPropertyName("previous_hash")]
    public string PreviousHash { get; set; } = string.Empty;

    [JsonPropertyName("reward")]
    public long Reward { get; set; }

    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;

}
