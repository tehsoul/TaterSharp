using Microsoft.Extensions.Options;
using Spectre.Console;
using TaterSharp.CLI.ApiModels;
using TaterSharp.CLI.Config;

namespace TaterSharp.CLI;

public interface IApp
{
    Task Run();
}
public class App : IApp
{
    private readonly IOptions<AppSettings> _appSettings;
    private readonly StarchOneApi _api;

    public App(IOptions<AppSettings> appSettings, StarchOneApi api)
    {
        _appSettings = appSettings;
        _api = api;
    }

    public async Task Run()
    {
        AnsiConsole.Write(
            new FigletText("TaterSharp")
                .Centered()
                .Color(ConsoleColor.Yellow));

        AnsiConsole.Write(new Rule($"[green]mining for companies {string.Join(", ", _appSettings.Value.CompanyIds)}[/]"));
        AnsiConsole.WriteLine();


        while (true)
        {
            foreach (var company in _appSettings.Value.CompanyIds)
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
                .Start($"Sleeping for {_appSettings.Value.SleepDelayInSeconds} seconds...", _ =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(_appSettings.Value.SleepDelayInSeconds));
                });



        }
        // ReSharper disable once FunctionNeverReturns
    }

    public async Task Mine(string companyId)
    {
        AnsiConsole.Write(new Rule($"[dim white]{DateTime.Now:yyyy-MM-dd HH:mm:ss}[/] [green]mining company {companyId}[/]"));
        var lastBlock = await _api.GetLastBlock();
        if (lastBlock is null)
        {
            AnsiConsole.WriteLine($"Couldn't get last hash info...");
            return;
        }

        var companyEmployees = await _api.GetCompanyEmployees(companyId);

        if (companyEmployees.Members.Count == 0)
        {
            AnsiConsole.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * Company {companyId} doesn't have any employees - sleeping and trying again later");
            return;
        }

        var blocksSubmissionRequest = new BlocksSubmissionRequest();

        foreach (string miner in companyEmployees.Members)
        {
            blocksSubmissionRequest.Blocks.Add(Solver.Solve(companyId, miner, lastBlock.Hash));
        }


        AnsiConsole.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * Submitting blocks for {companyEmployees.Members.Count} miners in companyId {companyId}...");
        var response = await _api.SubmitBlocks(blocksSubmissionRequest);

        var groupedByBlockStatus = response.GroupBy(x => x.Value.Status);
        foreach (var groupByBlockStatus in groupedByBlockStatus)
        {
            AnsiConsole.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * {groupByBlockStatus.Count()} miners {groupByBlockStatus.Key} ({string.Join(", ", groupByBlockStatus.Select(x => x.Key))})");
        }
    }
}
