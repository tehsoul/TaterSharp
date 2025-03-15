using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TaterSharp.CLI;
using TaterSharp.CLI.Config;
using TaterSharp.CLI.Mining;


var builder = new ConfigurationBuilder();
var configuration = builder.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((_, services) =>
    {
        // Add functionality to inject IOptions<T>
        services.AddOptions();

        // Add our Config object so it can be injected
        services.Configure<AppSettings>(configuration.GetSection(AppSettings.SectionKey));

        services.AddHttpClient("StarchOneApi", (sp, client) =>
        {
            client.BaseAddress = new Uri(sp.GetRequiredService<IOptions<AppSettings>>().Value.ApiHost);
        });

        // specify the factory for your class 
        services.AddTransient<StarchOneApi>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = factory.CreateClient("StarchOneApi");

            return new StarchOneApi(httpClient);
        });

        // register a miner for each company, we need to read the settings for this
        var appSettings = new AppSettings();
        configuration.GetSection(AppSettings.SectionKey).Bind(appSettings);
        foreach (var companyId in appSettings.CompanyIds)
        {
            services.AddKeyedSingleton<CompanyMiner>(companyId, (sp, key) => CompanyMiner.Create(sp.GetRequiredService<StarchOneApi>(), companyId));
            services.AddSingleton<CompanyMiner>(sp => sp.GetRequiredKeyedService<CompanyMiner>(companyId));
        }



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