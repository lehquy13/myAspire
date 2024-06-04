using Microsoft.Extensions.Options;
using Quartz;

namespace LibraryManagement.Infrastructure.BackgroundJobs.Configs;

public class ReportBackgroundJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = new JobKey(nameof(PdfReportBackgroundJob));
        options
            .AddJob<PdfReportBackgroundJob>(builder => builder.WithIdentity(jobKey))
            .AddTrigger(trigger => trigger.ForJob(jobKey)
                .WithIdentity(nameof(PdfReportBackgroundJob))
                .WithCronSchedule("0 59 23 * * ?"));
                //  .WithCronSchedule("0/30 * * * * ?"));
    }
}