namespace DuelSimulation.Class;

public class DuelBgService : BackgroundService
{    
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // TODO: implement logic

            await Task.Delay(10_000);
        }
    }
}