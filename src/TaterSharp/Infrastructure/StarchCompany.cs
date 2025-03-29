using TaterSharp.Application;
using TaterSharp.Common.ApiModels;
using TaterSharp.Common.Helpers;
using TaterSharp.Config;

namespace TaterSharp.Infrastructure;

public class StarchCompany
{
    private readonly StarchOneApi _api;
    private readonly IApplicationOutput _output;
    private OrdinalIgnoreCaseHashSet _employees = [];
    private long _lastKnownBlock = 0;
    private int _mineCounter = 0;

    public OrdinalIgnoreCaseHashSet Employees => _employees;

    public static StarchCompany Create(StarchOneApi api, CompanyConfiguration companyConfiguration, IApplicationOutput output)
    {
        return new StarchCompany(api, companyConfiguration, output);
    }

    public string CompanyId { get; private set; }
    public string Color { get; private set; }
    public bool ConfiguredToBeMined { get; private set; }

    private StarchCompany(StarchOneApi api, CompanyConfiguration companyConfiguration, IApplicationOutput output)
    {
        _api = api;
        _output = output;
        CompanyId = companyConfiguration.CompanyId;
        ConfiguredToBeMined = companyConfiguration.Mine;
        Color = $"#{companyConfiguration.CompanyId}";
    }

    public async Task UpdateEmployees()
    {
        var companyEmployees = await _api.GetCompanyEmployees(CompanyId);
        _employees = companyEmployees.Members;
    }

    public async Task Mine(LastBlockInfoResponse lastBlockInfo)
    {
        _output.WriteHeader($"Company: {CompanyId} ({_mineCounter} blocks this session)");

        await UpdateEmployees();

        if (Employees.Count == 0)
        {
            _output.WriteLine($"Company {CompanyId} doesn't have any employees - sleeping and trying again later");
            return;
        }

        var blocksSubmissionRequest = new BlocksSubmissionRequest();

        foreach (string employedMiner in Employees)
        {
            blocksSubmissionRequest.Blocks.Add(Solver.Solve(lastBlockInfo.Hash, employedMiner, Color));
        }

        _output.WriteLine($"Submitting blocks for {Employees.Count} miners in companyId {CompanyId}...");
        var response = await _api.SubmitBlocks(blocksSubmissionRequest);

        var groupedByBlockStatus = response.GroupBy(x => x.Value.Status);
        foreach (var groupByBlockStatus in groupedByBlockStatus)
        {
            _output.WriteLine($"{groupByBlockStatus.Count()} miners {groupByBlockStatus.Key} ({string.Join(", ", groupByBlockStatus.Select(x => x.Key))})");
        }

        // check if last block was mined by one of ours!
        if (!lastBlockInfo.BlockId.Equals(_lastKnownBlock))
        {
            if (Employees.Contains(lastBlockInfo.MinerId))
            {
                _output.WriteMinedBlockCelebration(CompanyId, lastBlockInfo.MinerId);
                _mineCounter++;
            }

            _lastKnownBlock = lastBlockInfo.BlockId;
        }

    }
}
