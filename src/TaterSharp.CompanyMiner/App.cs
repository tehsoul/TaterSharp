using Microsoft.Extensions.Options;
using Spectre.Console;
using TaterSharp.Application;
using TaterSharp.Config;
using TaterSharp.Infrastructure;

namespace TaterSharp.CompanyMiner;

public class App(IEnumerable<StarchCompany> companyMiners, IOptions<AppSettings> appSettings, StarchOneApi api, IApplicationOutput output) : IApp
{
    private readonly IOptions<AppSettings> _appSettings = appSettings;
    private readonly List<StarchCompany> _companyMiners = companyMiners.ToList();
    private readonly StarchOneApi _api = api;
    private readonly IApplicationOutput _output = output;

    public async Task Run()
    {
        _output.WriteApplicationStartup($"mining for companies {string.Join(", ", _companyMiners.Where(x => x.ConfiguredToBeMined).Select(x => x.CompanyId))}");

        while (true)
        {
            try
            {
                var lastBlockInfo = await _api.GetLastBlock();
                if (lastBlockInfo is null)
                {
                    continue;
                }

                foreach (var companyMiner in _companyMiners.Where(x => x.ConfiguredToBeMined))
                {
                    await companyMiner.Mine(lastBlockInfo);
                }
            }
            catch (Exception e)
            {
                _output.WriteException(e);
            }

            AnsiConsole.Status()
                .Start($"Sleeping for {_appSettings.Value.SleepDelayInSeconds} seconds...", _ =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(_appSettings.Value.SleepDelayInSeconds));
                });
        }
        // ReSharper disable once FunctionNeverReturns
    }
}
