using System.Text.Json.Serialization;

namespace TaterSharp.CLI.ApiModels;

public class CompanyEmployeesResponse
{
    [JsonPropertyName("members")] 
    public List<string> Members { get; set; } = [];
}