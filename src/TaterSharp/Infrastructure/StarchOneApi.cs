using System.ComponentModel.Design;
using System.Text;
using System.Text.Json;
using TaterSharp.Application;
using TaterSharp.Common.ApiModels;

namespace TaterSharp.Infrastructure;
public class StarchOneApi(HttpClient client, IApplicationOutput output)
{
    private readonly HttpClient _client = client;
    private readonly IApplicationOutput _output = output;

    private async Task<T?> GetStringAsAsync<T>(string requestUrl) where T : class
    {
        try
        {
            string responseString = await _client.GetStringAsync(requestUrl);
            if (!TryDeserialize<T>(responseString, out var deserialized))
            {
                return null;
            }
            return deserialized!;
        }
        catch (Exception e)
        {
            _output.WriteException(e);
            return null;
        }
    }

    public async Task<CompanyEmployeesResponse?> GetCompanyEmployees(string companyId)
    {
        return await GetStringAsAsync<CompanyEmployeesResponse>($"/teams/{companyId}/members");
    }

    public async Task<PendingBlocksResponse?> GetPendingBlocks()
    {
        return await GetStringAsAsync<PendingBlocksResponse>($"/pending_blocks");
    }

    public async Task<LastHashResponse?> GetLastHash()
    {
        return await GetStringAsAsync<LastHashResponse>($"/blockchain/last_hash");
    }
    
    public async Task<LastBlockInfoResponse?> GetLastBlock()
    {
        return await GetStringAsAsync<LastBlockInfoResponse>($"/blockchain/last_block");
    }

    public async Task<BlockInfoResponse?> GetBlockInfo(long blockId)
    {
        return await GetStringAsAsync<BlockInfoResponse>($"/blockchain/id/{blockId}");
    }

    public async Task<SystemTimeResponse?> GetSystemTime()
    {
        return await GetStringAsAsync<SystemTimeResponse>($"/system_time");
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
                return [];
            }

            return deserialized!;

        }
        catch (Exception e)
        {
            _output.WriteException(e);
            return [];
        }
    }

    public bool TryDeserialize<T>(string json, out T? deserialized)
    {
        if (string.IsNullOrEmpty(json))
        {
            deserialized = default;
            return false;
        }
            
        try
        {
            deserialized = JsonSerializer.Deserialize<T>(json);
            return true;
        }
        catch (JsonException jsonException)
        {
            _output.WriteDeserializationException(json, jsonException);
            

            deserialized = default;
            return false;
        }
    }
}
