using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaterSharp.Common.ApiModels;
public class BlockInfoResponse
{
    [JsonPropertyName("block_id")]
    public long BlockId { get; set; }

    [JsonPropertyName("color")]
    public string Color { get; set; }

    [JsonPropertyName("era")]
    public string Era { get; set; }

    [JsonPropertyName("hash")]
    public string Hash { get; set; }

    [JsonPropertyName("miner_id")]
    public string MinerId { get; set; }

    [JsonPropertyName("online")]
    public List<string> Online { get; set; }

    [JsonPropertyName("previous_hash")]
    public string PreviousHash { get; set; }

    [JsonPropertyName("reward")]
    public long Reward { get; set; }

    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; }

}
