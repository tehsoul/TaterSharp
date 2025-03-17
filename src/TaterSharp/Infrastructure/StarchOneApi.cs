using System.Text;
using System.Text.Json;
using Spectre.Console;
using Spectre.Console.Json;
using TaterSharp.Common.ApiModels;

namespace TaterSharp.Infrastructure;
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

    public async Task<PendingBlocksResponse?> GetPendingBlocks()
    {
        try
        {
            string responseString = await _client.GetStringAsync($"/pending_blocks");
            if (!TryDeserialize<PendingBlocksResponse>(responseString, out var deserialized))
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
    
    public async Task<LastBlockInfoResponse?> GetLastBlock()
    {
        try
        {
            string responseString = await _client.GetStringAsync($"/blockchain/last_block");

            if (!TryDeserialize<LastBlockInfoResponse>(responseString, out var deserialized))
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

    public async Task<BlockInfoResponse?> GetBlockInfo(long blockId)
    {
        try
        {
            string responseString = await _client.GetStringAsync($"/blockchain/id/{blockId}");

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

    public async Task<SystemTimeResponse?> GetSystemTime()
    {
        try
        {
            string responseString = await _client.GetStringAsync($"/system_time");

            if (!TryDeserialize<SystemTimeResponse>(responseString, out var deserialized))
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
