using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TaterSharp.Application;
using TaterSharp.Config;
using TaterSharp.Infrastructure;
using TaterSharp.Output;

namespace TaterSharp;

public static class ServiceExtensions
{
    public static IServiceCollection AddTaterSharp(
        this IServiceCollection services, IConfiguration configuration)
    {
        // Add functionality to inject IOptions<T>
        services.AddOptions();
        // Add our Config object so it can be injected
        services.Configure<AppSettings>(configuration.GetSection(AppSettings.SectionKey));

        services.AddSingleton<IApplicationOutput, ConsoleOutput>();

        services.AddHttpClient("StarchOneApi", (sp, client) =>
        {
            client.BaseAddress = new Uri(sp.GetRequiredService<IOptions<AppSettings>>().Value.ApiHost);
        });

        // specify the factory for your class 
        services.AddTransient<StarchOneApi>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = factory.CreateClient("StarchOneApi");

            return new StarchOneApi(httpClient, sp.GetRequiredService<IApplicationOutput>());
        });

        // register a miner for each company, we need to read the settings for this
        var appSettings = new AppSettings();
        configuration.GetSection(AppSettings.SectionKey).Bind(appSettings);
        foreach (var companyConfiguration in appSettings.Companies)
        {
            if (string.IsNullOrEmpty(companyConfiguration.CompanyId))
            {
                continue;
            }

            services.AddKeyedSingleton<StarchCompany>(companyConfiguration.CompanyId, (sp, key) => StarchCompany.Create(sp.GetRequiredService<StarchOneApi>(), companyConfiguration, sp.GetRequiredService<IApplicationOutput>()));
            services.AddSingleton<StarchCompany>(sp => sp.GetRequiredKeyedService<StarchCompany>(companyConfiguration.CompanyId));
        }
        return services;
    }
}
