using Microsoft.Extensions.Options;
using Spectre.Console;
using TaterSharp.CLI.Config;
using TaterSharp.CLI.Mining;

namespace TaterSharp.CLI;

public class App : IApp
{
    private readonly IOptions<AppSettings> _appSettings;
    private readonly List<CompanyMiner> _companyMiners;

    public App(IEnumerable<CompanyMiner> companyMiners, IOptions<AppSettings> appSettings)
    {
        _companyMiners = companyMiners.ToList();
        _appSettings = appSettings;
    }

    public async Task Run()
    {
        AnsiConsole.Write(
            new FigletText("TaterSharp")
                .Centered()
                .Color(ConsoleColor.Yellow));

        AnsiConsole.Write(new Rule($"[green]mining for companies {string.Join(", ", _companyMiners.Select(x=>x.CompanyId))}[/]"));
        AnsiConsole.WriteLine();


        while (true)
        {
            foreach (var companyMiner in _companyMiners)
            {
                try
                {
                    await companyMiner.Mine();
                }
                catch (Exception e)
                {
                    AnsiConsole.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * ERROR while mining for {companyMiner.CompanyId}");
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
}
