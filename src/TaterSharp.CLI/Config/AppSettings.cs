using System.Text.Json.Serialization;
using TaterSharp.CLI.Helpers;

namespace TaterSharp.CLI.Config;
public class AppSettings
{
    public const string SectionKey = "appsettings";

    [JsonPropertyName("companyIds")] 
    public OrdinalIgnoreCaseHashSet CompanyIds { get; set; } = [];

    [JsonPropertyName("apiHost")]
    public string ApiHost { get; set; } = "https://api.starch.one";

    [JsonPropertyName("sleepDelayInSeconds")]
    public int SleepDelayInSeconds { get; set; } = 30;
}
