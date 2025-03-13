using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaterSharp.CLI.Models;
public class BlocksSubmissionRequest
{
    [JsonPropertyName("blocks")] 
    public List<SingleBlockSubmission> Blocks { get; set; } = [];
}

public partial class SingleBlockSubmission
{
    [JsonPropertyName("hash")]
    public string Hash { get; set; }

    [JsonPropertyName("miner_id")]
    public string MinerId { get; set; }

    [JsonPropertyName("color")]
    public string Color { get; set; }
}
