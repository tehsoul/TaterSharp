using System.Text.Json.Serialization;

namespace TaterSharp.CLI.Models
{
    public class BlocksSubmissionResponse
    {
        [JsonPropertyName("block")]
        public string Status { get; set; }
    }
}
