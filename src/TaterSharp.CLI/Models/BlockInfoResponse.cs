using System.Text.Json.Serialization;

namespace TaterSharp.CLI.Models;

public class BlockInfoResponse
{
    [JsonPropertyName("block_id")]
    public long BlockId { get; set; }

    [JsonPropertyName("color")]
    public string Color { get; set; }

    [JsonPropertyName("hash")]
    public string Hash { get; set; }

    [JsonPropertyName("miner_id")]
    public string MinerId { get; set; }

    [JsonPropertyName("previous_hash")]
    public string PreviousHash { get; set; }
}