using Spectre.Console;
using TaterSharp.CLI.ApiModels;

namespace TaterSharp.CLI.Mining;
public class CompanyMiner
{
    private readonly StarchOneApi _api;
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

    public async Task Mine()
    {
        AnsiConsole.Write(new Rule($"[dim white]{DateTime.Now:yyyy-MM-dd HH:mm:ss}[/] [green]mining company {CompanyId}[/]"));
        var lastBlock = await _api.GetLastBlock();
        if (lastBlock is null)
        {
            AnsiConsole.WriteLine($"Couldn't get last hash info...");
            return;
        }

        var companyEmployees = await _api.GetCompanyEmployees(CompanyId);

        if (companyEmployees.Members.Count == 0)
        {
            AnsiConsole.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * Company {CompanyId} doesn't have any employees - sleeping and trying again later");
            return;
        }

        var blocksSubmissionRequest = new BlocksSubmissionRequest();

        foreach (string miner in companyEmployees.Members)
        {
            blocksSubmissionRequest.Blocks.Add(Solver.Solve(lastBlock.Hash, miner, Color));
        }


        AnsiConsole.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * Submitting blocks for {companyEmployees.Members.Count} miners in companyId {CompanyId}...");
        var response = await _api.SubmitBlocks(blocksSubmissionRequest);

        var groupedByBlockStatus = response.GroupBy(x => x.Value.Status);
        foreach (var groupByBlockStatus in groupedByBlockStatus)
        {
            AnsiConsole.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * {groupByBlockStatus.Count()} miners {groupByBlockStatus.Key} ({string.Join(", ", groupByBlockStatus.Select(x => x.Key))})");
        }
    }
}
