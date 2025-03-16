using Spectre.Console;
using TaterSharp.Common.ApiModels;
using TaterSharp.Common.Helpers;
using TaterSharp.Config;

namespace TaterSharp.Infrastructure;

public class StarchCompany
{
    private readonly StarchOneApi _api;
    private OrdinalIgnoreCaseHashSet _employees = [];
    private long _lastKnownBlock = 0;
    private int _mineCounter = 0;

    public OrdinalIgnoreCaseHashSet Employees => _employees;

    public static StarchCompany Create(StarchOneApi api, CompanyConfiguration companyConfiguration)
    {
        return new StarchCompany(api, companyConfiguration);
    }

    public string CompanyId { get; private set; }
    public string Color { get; private set; }
    public bool ConfiguredToBeMined { get; private set; }

    private StarchCompany(StarchOneApi api, CompanyConfiguration companyConfiguration)
    {
        _api = api;
        CompanyId = companyConfiguration.CompanyId;
        ConfiguredToBeMined = companyConfiguration.Mine;
        Color = $"#{companyConfiguration.CompanyId}";
    }

    public async Task UpdateEmployees()
    {
        var companyEmployees = await _api.GetCompanyEmployees(CompanyId);
        _employees = companyEmployees.Members;
    }

    public async Task Mine(LastBlockInfoResponse lastLastBlockInfo)
    {
        AnsiConsole.Write(new Rule($"[dim white]{DateTime.Now:yyyy-MM-dd HH:mm:ss}[/] [green]Company: {CompanyId} ({_mineCounter} blocks this session)[/]"));

        await UpdateEmployees();

        if (Employees.Count == 0)
        {
            AnsiConsole.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * Company {CompanyId} doesn't have any employees - sleeping and trying again later");
            return;
        }

        var blocksSubmissionRequest = new BlocksSubmissionRequest();

        foreach (string employedMiner in Employees)
        {
            blocksSubmissionRequest.Blocks.Add(Solver.Solve(lastLastBlockInfo.Hash, employedMiner, Color));
        }


        AnsiConsole.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * Submitting blocks for {Employees.Count} miners in companyId {CompanyId}...");
        var response = await _api.SubmitBlocks(blocksSubmissionRequest);

        var groupedByBlockStatus = response.GroupBy(x => x.Value.Status);
        foreach (var groupByBlockStatus in groupedByBlockStatus)
        {
            AnsiConsole.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * {groupByBlockStatus.Count()} miners {groupByBlockStatus.Key} ({string.Join(", ", groupByBlockStatus.Select(x => x.Key))})");
        }

        // check if last block was mined by one of ours!
        if (!lastLastBlockInfo.BlockId.Equals(_lastKnownBlock))
        {
            if (Employees.Contains(lastLastBlockInfo.MinerId))
            {
                AnsiConsole.Write(new Rule($"[green]Yay! a miner of company {CompanyId} mined a block![/]"));
                // hooray
                AnsiConsole.Write(
                    new FigletText(lastLastBlockInfo.MinerId)
                        .Centered()
                        .Color(ConsoleColor.Green));

                AnsiConsole.Write(new Rule($"[green]Congratulations {lastLastBlockInfo.MinerId}[/]"));
                _mineCounter++;
            }

            _lastKnownBlock = lastLastBlockInfo.BlockId;
        }

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

        await Mine(lastBlockInfo);
    }
}
