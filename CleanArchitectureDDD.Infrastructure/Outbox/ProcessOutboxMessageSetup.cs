using Microsoft.Extensions.Options;
using Quartz;

namespace CleanArchitectureDDD.Infrastructure.Outbox;

public class ProcessOutboxMessageSetup: IConfigureOptions<QuartzOptions>
{
    private readonly OutboxOptions _outboxOptions;

    public ProcessOutboxMessageSetup(IOptions<OutboxOptions> outboxOptions)
    {
        _outboxOptions = outboxOptions.Value;
    }

    //InvokeOutboxMessagesJob
    public void Configure(QuartzOptions options)
    {
        const string jobName = nameof(InvokeOutboxMessagesJob);

        options.AddJob<InvokeOutboxMessagesJob>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure =>
            {
                configure
                    .ForJob(jobName)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInSeconds(_outboxOptions.IntervalInSeconds)
                            .RepeatForever()
                    );
            });
    }
}