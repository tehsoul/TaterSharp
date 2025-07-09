using Microsoft.Extensions.Options;
using Spectre.Console;
using TaterSharp.Application;
using TaterSharp.Config;
using TaterSharp.Infrastructure;

namespace TaterSharp.CompanyMiner;

public class App : IApp
{
    private readonly List<StarchCompany> _companyMiners;
    private readonly IOptions<AppSettings> _appSettings;
    private readonly StarchOneApi _api;
    private readonly IApplicationOutput _output;

    public App(IEnumerable<StarchCompany> companyMiners, IOptions<AppSettings> appSettings, StarchOneApi api, IApplicationOutput output)
    {
        _appSettings = appSettings;
        _api = api;
        _output = output;
        _companyMiners = companyMiners.ToList();
    }

    public async Task Run()
    {
        var companyInfo =
            $"mining for companies {string.Join(", ", _companyMiners.Where(x => x.ConfiguredToBeMined).Select(x => x.CompanyId))}";
        var apiInfo = $"using api: {_appSettings.Value.ApiHost}";
        _output.WriteApplicationStartup(companyInfo, apiInfo);

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
