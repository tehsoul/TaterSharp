using Spectre.Console;
using Spectre.Console.Json;
using System.Text.Json;
using TaterSharp.Application;

namespace TaterSharp.Output;
public class ConsoleOutput : IApplicationOutput
{
    public void WriteHeader(string text, bool includeTimestamp = true)
    {
        text = includeTimestamp ? $"[dim white]{DateTime.Now:yyyy-MM-dd HH:mm:ss}[/] [green]{text}[/]" : $"[green]{text}[/]";
        AnsiConsole.Write(new Rule(text));
    }

    public void WriteLine(string text, bool includeTimestamp = true)
    {
        text = includeTimestamp ? $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} * {text}" : text;
        AnsiConsole.WriteLine(text);
    }

    public void WriteMinedBlockCelebration(string companyId, string minerId)
    {
        AnsiConsole.Write(new Rule($"[green]Yay! a miner of company {companyId} mined a block![/]"));
        // hooray
        AnsiConsole.Write(
            new FigletText(minerId)
                .Centered()
                .Color(ConsoleColor.Green));

        AnsiConsole.Write(new Rule($"[green]Congratulations {minerId}[/]"));
    }

    public void WriteApplicationStartup(string companyInfo, string apiInfo)
    {
        AnsiConsole.Write(
            new FigletText("TaterSharp")
                .Centered()
                .Color(ConsoleColor.Yellow));

        AnsiConsole.Write(new Rule($"[green]{companyInfo}[/]"));
        AnsiConsole.Write(new Rule($"[green]{apiInfo}[/]"));
        AnsiConsole.WriteLine();
    }

    public void WriteException(Exception exception)
    {
        AnsiConsole.WriteException(exception);
    }

    public void WriteDeserializationException(string json, JsonException jsonException)
    {
        AnsiConsole.Write($"Error deserializing json:");
        AnsiConsole.WriteException(jsonException);
        try
        {
            AnsiConsole.Write(new JsonText(json));
        }
        catch
        {
            // ignored
        }
    }
}