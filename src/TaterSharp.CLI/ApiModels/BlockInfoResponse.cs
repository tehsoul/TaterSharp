﻿using System.Text.Json.Serialization;

namespace TaterSharp.CLI.ApiModels;

public class BlockInfoResponse
{
    [JsonPropertyName("block_id")]
    public long BlockId { get; set; }

    [JsonPropertyName("color")] 
    public string Color { get; set; } = string.Empty;

    [JsonPropertyName("hash")] 
    public string Hash { get; set; } = string.Empty;

    [JsonPropertyName("miner_id")]
    public string MinerId { get; set; } = string.Empty;

    [JsonPropertyName("previous_hash")]
    public string PreviousHash { get; set; } = string.Empty;
}