using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Json;
using TaterSharp.CLI.ApiModels;
using TaterSharp.CLI.Config;

namespace TaterSharp.CLI;
public class StarchOneApi
{
    private readonly HttpClient _client;

    public StarchOneApi(HttpClient client)
    {
        this._client = client;
    }

    public async Task<CompanyEmployeesResponse> GetCompanyEmployees(string companyId)
    {
        try
        {
            string responseString = await _client.GetStringAsync($"/teams/{companyId}/members");
            if (!TryDeserialize<CompanyEmployeesResponse>(responseString, out var deserialized))
            {
                return new CompanyEmployeesResponse();
            }
            return deserialized!;
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            return new CompanyEmployeesResponse();
        }
    }
   
    public async Task<LastHashResponse?> GetLastHash()
    {
        try
        {
            string responseString = await _client.GetStringAsync($"/blockchain/last_hash");
            if (!TryDeserialize<LastHashResponse>(responseString, out var deserialized))
            {
                return null;
            }
            return deserialized!;
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            return null;
        }
    }

    public async Task<BlockInfoResponse?> GetLastBlock()
    {
        try
        {
            string responseString = await _client.GetStringAsync($"/blockchain/last_block");

            if (!TryDeserialize<BlockInfoResponse>(responseString, out var deserialized))
            {
                return null;
            }
            return deserialized!;
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            return null;
        }
    }



    public async Task<Dictionary<string, BlocksSubmissionResponse>> SubmitBlocks(BlocksSubmissionRequest request)
    {
        try
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync($"/submit_blocks", content);
            string responseString = await response.Content.ReadAsStringAsync();

            if (!TryDeserialize<Dictionary<string, BlocksSubmissionResponse>>(responseString, out var deserialized))
            {
                return new Dictionary<string, BlocksSubmissionResponse>();
            }

            return deserialized!;

        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            return new Dictionary<string, BlocksSubmissionResponse>();
        }
    }

    public bool TryDeserialize<T>(string json, out T? deserialized)
    {
        if (string.IsNullOrEmpty(json))
        {
            deserialized = default(T);
            return false;
        }
            
        try
        {
            deserialized = JsonSerializer.Deserialize<T>(json);
            return true;
        }
        catch (JsonException jsonException)
        {
            AnsiConsole.Write($"Error deserializing json:");
            AnsiConsole.WriteException(jsonException);
            try
            {
                AnsiConsole.Write(new JsonText(json));
            }
            catch
            {
                // ignored
            }

            deserialized = default(T);
            return false;
        }
    }
}
