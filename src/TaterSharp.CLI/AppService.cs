using Microsoft.Extensions.Hosting;

namespace TaterSharp.CLI;

public class AppService : BackgroundService
{
    private readonly IApp _app;

    public AppService(IApp app)
    {
        this._app = app;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await _app.Run();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}