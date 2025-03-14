using System.Text.Json.Serialization;

namespace TaterSharp.CLI.ApiModels;
public class BlocksSubmissionRequest
{
    [JsonPropertyName("blocks")] 
    public List<SingleBlockSubmission> Blocks { get; set; } = [];
}

public partial class SingleBlockSubmission
{
    [JsonPropertyName("hash")]
    public string Hash { get; set; } = string.Empty;

    [JsonPropertyName("miner_id")]
    public string MinerId { get; set; } = string.Empty;

    [JsonPropertyName("color")]
    public string Color { get; set; } = string.Empty;
}
