using System.Diagnostics;
using Spectre.Console;
using TaterSharp.CLI.Models;

namespace TaterSharp.CLI;

class Program
{
    private static readonly StarchOneApi Api = new(new HttpClient());
    private static readonly List<string> CompanyIdsToMine = new() { "EDD336", "0CF33C" };
    private const int SleepDelayInSeconds = 45;

    static async Task Main()
    {
        AnsiConsole.Write(
            new FigletText("TaterSharp")
                .Centered()
                .Color(System.ConsoleColor.Yellow));

        AnsiConsole.Write(new Rule($"[green]mining for companies {string.Join(", ", CompanyIdsToMine)}[/]"));
        AnsiConsole.WriteLine();


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
                    AnsiConsole.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * ERROR while mining for {company}");
                    AnsiConsole.WriteException(e);
                }
            }
            AnsiConsole.Status()
                .Start($"Sleeping for {SleepDelayInSeconds} seconds...", ctx =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(SleepDelayInSeconds)); // Sleep for 45 seconds
                });


            
        }
        // ReSharper disable once FunctionNeverReturns
    }

    static async Task Mine(string companyId)
    {
        AnsiConsole.Write(new Rule($"[dim white]{DateTime.Now:yyyy-MM-dd HH:mm:ss}[/] [green]mining company {companyId}[/]"));
        var lastHashResponse = await Api.GetLastHash();
        if (lastHashResponse is null)
        {
            AnsiConsole.WriteLine($"Couldn't get last hash info...");
            return;
        }

        var companyEmployees = await Api.GetCompanyEmployees(companyId);

        if (companyEmployees.Members.Count == 0)
        {
            AnsiConsole.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * Company {companyId} doesn't have any employees - sleeping and trying again later");
            return;
        }

        var blocksSubmissionRequest = new BlocksSubmissionRequest();

        foreach (string miner in companyEmployees.Members)
        {
            blocksSubmissionRequest.Blocks.Add(Solver.Solve(companyId, miner, lastHashResponse.Hash));
        }


        AnsiConsole.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * Submitting blocks for {companyEmployees.Members.Count} miners in companyId {companyId}...");
        var response = await Api.SubmitBlocks(blocksSubmissionRequest);

        var groupedByBlockStatus = response.GroupBy(x => x.Value.Status);
        foreach (var groupByBlockStatus in groupedByBlockStatus)
        {
            AnsiConsole.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * {groupByBlockStatus.Count()} miners {groupByBlockStatus.Key} ({string.Join(", ", groupByBlockStatus.Select(x => x.Key))})");
        }
    }
}