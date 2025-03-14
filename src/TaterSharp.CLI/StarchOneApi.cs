using System.Text;
using System.Text.Json;
using TaterSharp.CLI.ApiModels;

namespace TaterSharp.CLI;
public class StarchOneApi
{
    private readonly HttpClient _client;
    private const string ApiHost = "https://api.starch.one";

    public StarchOneApi(HttpClient client)
    {
        this._client = client;
    }

    public async Task<CompanyEmployeesResponse> GetCompanyEmployees(string companyId)
    {
        try
        {
            string response = await _client.GetStringAsync($"{ApiHost}/teams/{companyId}/members");
            return JsonSerializer.Deserialize<CompanyEmployeesResponse>(response) ?? new CompanyEmployeesResponse();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new CompanyEmployeesResponse();
        }
    }
   
    public async Task<LastHashResponse?> GetLastHash()
    {
        try
        {
            string response = await _client.GetStringAsync($"{ApiHost}/blockchain/last_hash");
            return JsonSerializer.Deserialize<LastHashResponse>(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<BlockInfoResponse?> GetLastBlock()
    {
        try
        {
            string response = await _client.GetStringAsync($"{ApiHost}/blockchain/last_block");
            return JsonSerializer.Deserialize<BlockInfoResponse>(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }



    public async Task<Dictionary<string, BlocksSubmissionResponse>> SubmitBlocks(BlocksSubmissionRequest request)
    {
        try
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync($"{ApiHost}/submit_blocks", content);
            string responseString = await response.Content.ReadAsStringAsync();

            //var jsonLog = new JsonText(json);
            //AnsiConsole.Write(
            //    new Panel(jsonLog)
            //        .Header("Block submission!")
            //        .Collapse()
            //        .RoundedBorder()
            //        .BorderColor(Color.Yellow));

            return JsonSerializer.Deserialize<Dictionary<string, BlocksSubmissionResponse>>(responseString) ?? new Dictionary<string, BlocksSubmissionResponse>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Dictionary<string, BlocksSubmissionResponse>();
        }
    }
}
