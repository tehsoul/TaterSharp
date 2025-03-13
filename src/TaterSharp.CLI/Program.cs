using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TaterSharp.CLI.Models;

namespace TaterSharp.CLI;

class Program
{
    static readonly HttpClient client = new HttpClient();
    static readonly string APIHOST = "https://api.starch.one";
    static readonly string COMPANY_ID = "868C0C";
    static readonly string COLOR = $"#{COMPANY_ID}";

    static async Task<CompanyEmployeesResponse> GetCompanyEmployees()
    {
        try
        {
            string response = await client.GetStringAsync($"{APIHOST}/teams/{COMPANY_ID}/members");
            return JsonSerializer.Deserialize<CompanyEmployeesResponse>(response) ?? new CompanyEmployeesResponse();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new CompanyEmployeesResponse();
        }
    }

    static async Task<BlockInfoResponse?> GetLastBlock()
    {
        try
        {
            string response = await client.GetStringAsync($"{APIHOST}/blockchain/last_block");
            return JsonSerializer.Deserialize<BlockInfoResponse>(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    static SingleBlockSubmission Solve(string minerId, BlockInfoResponse lastBlock)
    {
        string stringToHash = $"{lastBlock.Hash} {minerId} {COLOR}";

        var hash = GetSha256HexDigest(stringToHash);

        return new SingleBlockSubmission
            {
                Hash = hash,
                MinerId = minerId,
                Color = COLOR
            };
    }

    static string GetSha256HexDigest(string stringToHash)
    {
        using SHA256 sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }
        return builder.ToString();
    }

    static async Task<Dictionary<string, BlocksSubmissionResponse>> SubmitBlocks(BlocksSubmissionRequest request)
    {
        try
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{APIHOST}/submit_blocks", content);
            string responseString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Dictionary<string, BlocksSubmissionResponse>>(responseString);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Dictionary<string, BlocksSubmissionResponse>();
        }
    }

    static async Task Mine()
    {
        var lastBlock = await GetLastBlock();
        if (lastBlock is null)
        {
            return;
        }

        var companyEmployees = await GetCompanyEmployees();

        var blocksSubmissionRequest = new BlocksSubmissionRequest();

        foreach (string miner in companyEmployees.Members)
        {
            blocksSubmissionRequest.Blocks.Add(Solve(miner, lastBlock));
        }

        System.Console.WriteLine($"-------------------");
        System.Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} * Submitting blocks for {companyEmployees.Members.Count} miners in company {COMPANY_ID}...");
        var response = await SubmitBlocks(blocksSubmissionRequest);

        var groupedByBlockStatus = response.GroupBy(x => x.Value.Status);
        foreach (var groupByBlockStatus in groupedByBlockStatus)
        {
            Console.WriteLine($"- {groupByBlockStatus.Count()} miners {groupByBlockStatus.Key} ({string.Join(", ", groupByBlockStatus.Select(x=>x.Key))})");
        }
    }

    static async Task Main()
    {
        Console.WriteLine($"TaterSharp : mining for {COMPANY_ID}");

        while (true)
        {
            try
            {
                await Mine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Thread.Sleep(49000); // Sleep for 49 seconds
        }
    }
}