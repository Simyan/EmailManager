using EmailManager.Application;

namespace EmailManager.Processor;

public class Worker(ILogger<Worker> logger, IEmailManagerServiceForProcessor service) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await service.ProcessPendingRequests();
            logger.LogInformation("Worker finished at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000000, stoppingToken);
        }
    }
}