using Spectre.Console;
using TaterSharp.CLI.ApiModels;
using TaterSharp.CLI.Helpers;

namespace TaterSharp.CLI.Mining;
public class CompanyMiner
{
    private readonly StarchOneApi _api;
    private OrdinalIgnoreCaseHashSet _employees = [];
    private long _lastKnownBlock = 0;

    public OrdinalIgnoreCaseHashSet Employees => _employees;

    public static CompanyMiner Create(StarchOneApi api, string companyId)
    {
        return new CompanyMiner(api, companyId);
    }

    public string CompanyId { get; private set; }
    public string Color { get; private set; }
    private CompanyMiner(StarchOneApi api, string companyId)
    {
        _api = api;
        CompanyId = companyId;
        Color = $"#{companyId}";
    }

    public async Task UpdateEmployees()
    {
        var companyEmployees = await _api.GetCompanyEmployees(CompanyId);
        _employees = companyEmployees.Members;
    }

    public async Task Mine()
    {
        AnsiConsole.Write(new Rule($"[dim white]{DateTime.Now:yyyy-MM-dd HH:mm:ss}[/] [green]mining company {CompanyId}[/]"));
        var lastBlockInfo = await _api.GetLastBlock();
        if (lastBlockInfo is null)
        {
            AnsiConsole.WriteLine($"Couldn't get last hash info...");
            return;
        }

        await UpdateEmployees();

        if (Employees.Count == 0)
        {
            AnsiConsole.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * Company {CompanyId} doesn't have any employees - sleeping and trying again later");
            return;
        }

        var blocksSubmissionRequest = new BlocksSubmissionRequest();

        foreach (string employedMiner in Employees)
        {
            blocksSubmissionRequest.Blocks.Add(Solver.Solve(lastBlockInfo.Hash, employedMiner, Color));
        }


        AnsiConsole.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * Submitting blocks for {Employees.Count} miners in companyId {CompanyId}...");
        var response = await _api.SubmitBlocks(blocksSubmissionRequest);

        var groupedByBlockStatus = response.GroupBy(x => x.Value.Status);
        foreach (var groupByBlockStatus in groupedByBlockStatus)
        {
            AnsiConsole.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * {groupByBlockStatus.Count()} miners {groupByBlockStatus.Key} ({string.Join(", ", groupByBlockStatus.Select(x => x.Key))})");
        }

        // check if last block was mined by one of ours!
        if (!lastBlockInfo.BlockId.Equals(_lastKnownBlock))
        {
            if (Employees.Contains(lastBlockInfo.MinerId))
            {
                AnsiConsole.Write(new Rule($"[green]Yay! a miner of company {CompanyId} mined a block![/]"));
                // hooray
                AnsiConsole.Write(
                    new FigletText(lastBlockInfo.MinerId)
                        .Centered()
                        .Color(ConsoleColor.Green));

                AnsiConsole.Write(new Rule($"[green]Congratulations {lastBlockInfo.MinerId}[/]"));
            }

            _lastKnownBlock = lastBlockInfo.BlockId;
        }

    }
}
