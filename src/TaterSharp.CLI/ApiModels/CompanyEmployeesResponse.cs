using System.Text.Json.Serialization;
using TaterSharp.CLI.Helpers;

namespace TaterSharp.CLI.ApiModels;

public class CompanyEmployeesResponse
{
    [JsonPropertyName("members")] 
    public OrdinalIgnoreCaseHashSet Members { get; set; } = [];
}