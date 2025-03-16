using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TaterSharp.Config;
using TaterSharp.Infrastructure;

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
            services.AddKeyedSingleton<StarchCompany>(companyId, (sp, key) => StarchCompany.Create(sp.GetRequiredService<StarchOneApi>(), companyId));
            services.AddSingleton<StarchCompany>(sp => sp.GetRequiredKeyedService<StarchCompany>(companyId));
        }
        return services;
    }
}
