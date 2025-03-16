using Microsoft.Extensions.Options;
using Spectre.Console;
using TaterSharp.Config;
using TaterSharp.Infrastructure;

namespace TaterSharp.CompanyMiner;

public class App : IApp
{
    private readonly IOptions<AppSettings> _appSettings;
    private readonly List<StarchCompany> _companyMiners;
    private readonly StarchOneApi _api;

    public App(IEnumerable<StarchCompany> companyMiners, IOptions<AppSettings> appSettings, StarchOneApi api)
    {
        _companyMiners = companyMiners.ToList();
        _appSettings = appSettings;
        _api = api;
    }

    public async Task Run()
    {
        AnsiConsole.Write(
            new FigletText("TaterSharp")
                .Centered()
                .Color(ConsoleColor.Yellow));

        AnsiConsole.Write(new Rule($"[green]mining for companies {string.Join(", ", _companyMiners.Where(x => x.ConfiguredToBeMined).Select(x => x.CompanyId))}[/]"));
        AnsiConsole.WriteLine();


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
                AnsiConsole.WriteException(e);
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
