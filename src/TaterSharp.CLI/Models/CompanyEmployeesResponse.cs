using System.Text.Json.Serialization;

namespace TaterSharp.CLI.Models;

public class CompanyEmployeesResponse
{
    [JsonPropertyName("members")] 
    public List<string> Members { get; set; } = [];
}