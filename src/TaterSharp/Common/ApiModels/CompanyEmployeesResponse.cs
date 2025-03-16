using System.Text.Json.Serialization;
using TaterSharp.Common.Helpers;

namespace TaterSharp.Common.ApiModels;

public class CompanyEmployeesResponse
{
    [JsonPropertyName("members")] 
    public OrdinalIgnoreCaseHashSet Members { get; set; } = [];
}