using System.Text.Json.Serialization;

namespace TaterSharp.Config;

public class CompanyConfiguration
{
    [JsonPropertyName("companyId")]
    public string CompanyId { get; set; } = string.Empty;

    [JsonPropertyName("mine")]
    public bool Mine { get; set; } = false;

}