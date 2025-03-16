using System.Text.Json.Serialization;

namespace TaterSharp.Common.ApiModels
{
    public class BlocksSubmissionResponse
    {
        [JsonPropertyName("block")]
        public string Status { get; set; } = string.Empty;
    }
}
