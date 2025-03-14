using System.Text.Json.Serialization;

namespace TaterSharp.CLI.ApiModels
{
    public class BlocksSubmissionResponse
    {
        [JsonPropertyName("block")]
        public string Status { get; set; } = string.Empty;
    }
}
