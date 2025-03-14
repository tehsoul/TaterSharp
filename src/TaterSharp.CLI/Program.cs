using System.Diagnostics;
using TaterSharp.CLI.Models;

namespace TaterSharp.CLI;

class Program
{
    private static readonly StarchOneApi Api = new(new HttpClient());
    private static readonly List<string> CompanyIdsToMine = new() { "EDD336", "0CF33C" };

    static async Task Main()
    {
        Console.WriteLine($"TaterSharp : mining for companies {string.Join(", ", CompanyIdsToMine)}");

        while (true)
        {

            foreach (var company in CompanyIdsToMine)
            {
                try
                {
                    await Mine(company);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * ERROR while mining for {company}");
                    Console.WriteLine(e);
                }
            }
            Thread.Sleep(45000); // Sleep for 45 seconds
        }
        // ReSharper disable once FunctionNeverReturns
    }

    static async Task Mine(string companyId)
    {
        Console.WriteLine($"-------------------");
        var lastHashResponse = await Api.GetLastHash();
        if (lastHashResponse is null)
        {
            Console.WriteLine($"Couldn't get last hash info...");
            return;
        }

        var companyEmployees = await Api.GetCompanyEmployees(companyId);

        if (companyEmployees.Members.Count == 0)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * Company {companyId} doesn't have any employees - sleeping and trying again later");
            return;
        }

        var blocksSubmissionRequest = new BlocksSubmissionRequest();

        foreach (string miner in companyEmployees.Members)
        {
            blocksSubmissionRequest.Blocks.Add(Solver.Solve(companyId, miner, lastHashResponse.Hash));
        }


        Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * Submitting blocks for {companyEmployees.Members.Count} miners in companyId {companyId}...");
        var response = await Api.SubmitBlocks(blocksSubmissionRequest);

        var groupedByBlockStatus = response.GroupBy(x => x.Value.Status);
        foreach (var groupByBlockStatus in groupedByBlockStatus)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * {groupByBlockStatus.Count()} miners {groupByBlockStatus.Key} ({string.Join(", ", groupByBlockStatus.Select(x => x.Key))})");
        }
    }
}