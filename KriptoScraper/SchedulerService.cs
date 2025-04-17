using Quartz;
using Quartz.Impl;

public class SchedulerService
{
    public async Task StartScheduler()
    {
        StdSchedulerFactory factory = new StdSchedulerFactory();
        IScheduler scheduler = await factory.GetScheduler();

        await scheduler.Start();

        IJobDetail job = JobBuilder.Create<SolanaScraperJob>().WithIdentity("kriptoJob", "group1").Build();

        ITrigger trigger = TriggerBuilder
            .Create()
            .WithIdentity("kriptoTrigger", "group1")
            .StartNow()
            .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever())
            .Build();

        await scheduler.ScheduleJob(job, trigger);
        Console.ReadLine();

    }
}