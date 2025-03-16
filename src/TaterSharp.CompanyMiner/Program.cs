using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaterSharp;
using TaterSharp.CompanyMiner;


var builder = new ConfigurationBuilder();
var configuration = builder.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((_, services) =>
    {
        services.AddTaterSharp(configuration);

        services.AddSingleton<IApp, App>();
        services.AddHostedService<AppService>();
    })
    .ConfigureLogging((_, logging) =>
    {
        // the createdefaultbuilder registers a vanilla console logger which pollutes the CLI output --> just remove it, we handle the output we want ourselves
        logging.ClearProviders();
    })
    .Build();

await host.RunAsync();

Console.WriteLine($"Program ended.");