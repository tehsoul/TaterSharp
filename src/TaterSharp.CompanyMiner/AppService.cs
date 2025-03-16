using Microsoft.Extensions.Hosting;

namespace TaterSharp.CompanyMiner;

public class AppService : BackgroundService
{
    private readonly IApp _app;

    public AppService(IApp app)
    {
        this._app = app;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _app.Run();
    }
}