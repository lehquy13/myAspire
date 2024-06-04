using Microsoft.Extensions.Options;
using Quartz;

namespace LibraryManagement.Infrastructure.BackgroundJobs.Configs;

public class LoggingBackgroundJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = new JobKey(nameof(LoggingBackgroundJob));
        options
            .AddJob<LoggingBackgroundJob>(builder => builder.WithIdentity(jobKey))
            .AddTrigger(trigger => trigger.ForJob(jobKey)
                .WithIdentity(nameof(LoggingBackgroundJob))
                .WithCronSchedule("1 0/2 * * * ?"));
    }
}