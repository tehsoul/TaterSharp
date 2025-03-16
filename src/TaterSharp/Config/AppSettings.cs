using System.Text.Json.Serialization;
using TaterSharp.Common.Helpers;

namespace TaterSharp.Config;
public class AppSettings
{
    public const string SectionKey = "appsettings";

    [JsonPropertyName("companies")] 
    public List<CompanyConfiguration> Companies { get; set; } = [];

    [JsonPropertyName("apiHost")]
    public string ApiHost { get; set; } = "https://api.starch.one";

    [JsonPropertyName("sleepDelayInSeconds")]
    public int SleepDelayInSeconds { get; set; } = 60;
}