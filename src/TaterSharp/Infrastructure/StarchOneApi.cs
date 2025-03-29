﻿using System.Text;
using System.Text.Json;
using TaterSharp.Application;
using TaterSharp.Common.ApiModels;

namespace TaterSharp.Infrastructure;
public class StarchOneApi(HttpClient client, IApplicationOutput output)
{
    private readonly HttpClient _client = client;
    private readonly IApplicationOutput _output = output;

    public async Task<CompanyEmployeesResponse> GetCompanyEmployees(string companyId)
    {
        try
        {
            string responseString = await _client.GetStringAsync($"/teams/{companyId}/members");
            if (!TryDeserialize<CompanyEmployeesResponse>(responseString, out var deserialized))
            {
                return new();
            }
            return deserialized!;
        }
        catch (Exception e)
        {
            _output.WriteException(e);
            return new();
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
            _output.WriteException(e);
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
            _output.WriteException(e);
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
            _output.WriteException(e);
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
            _output.WriteException(e);
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
            _output.WriteException(e);
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
