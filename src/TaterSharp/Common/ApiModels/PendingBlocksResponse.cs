using System.Text.Json.Serialization;

namespace TaterSharp.Common.ApiModels;
public class PendingBlocksResponse
{
    [JsonPropertyName("blocks")] 
    public List<PendingBlock> Blocks { get; set; } = [];
}

public class PendingBlock
{
    [JsonPropertyName("hash")]
    public string Hash { get; set; } = string.Empty;

    [JsonPropertyName("miner_id")]
    public string MinerId { get; set; } = string.Empty;

    [JsonPropertyName("color")]
    public string Color { get; set; } = string.Empty;
}
