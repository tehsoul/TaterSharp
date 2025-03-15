using System.Text.Json.Serialization;

namespace TaterSharp.CLI.Config;
public class AppSettings
{
    [JsonPropertyName("companyIds")] 
    public HashSet<string> CompanyIds { get; set; } = [];

    [JsonPropertyName("apiHost")]
    public string ApiHost { get; set; } = "https://api.starch.one";

    [JsonPropertyName("sleepDelayInSeconds")]
    public int SleepDelayInSeconds { get; set; } = 30;
}
