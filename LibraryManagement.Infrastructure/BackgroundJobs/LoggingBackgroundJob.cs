using Microsoft.Extensions.Logging;
using Quartz;

namespace LibraryManagement.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution] // Mark that this job can't be run in parallel
internal class LoggingBackgroundJob : IJob
{
    private readonly ILogger<LoggingBackgroundJob> _logger;
    private static int _count = 0;
    
    public LoggingBackgroundJob(ILogger<LoggingBackgroundJob> logger)
    {
        _logger = logger;
    }
            
    public async Task Execute(IJobExecutionContext context)
    {
        await Task.CompletedTask;
        _logger.LogInformation("Checking background service working: count {Count} at {DateTime}", _count, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
        _count++;
    }
}