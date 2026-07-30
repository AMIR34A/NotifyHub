using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotifyHub.Infrastructure.Workers.Notifications;
using Polly;
using Polly.Retry;

namespace NotifyHub.Infrastructure.Workers;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureWorkers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddResiliencePipeline("retry-pipeline", builder =>
        {
            builder
                .AddRetry(new RetryStrategyOptions()
                {
                    Delay = TimeSpan.FromSeconds(5),
                    MaxRetryAttempts = 2,
                    UseJitter = true
                })
                .AddTimeout(TimeSpan.FromSeconds(10));
        });

        services.AddHostedService<SendNotificationWorker>();
        services.AddHostedService<RetrySendingNotificationWorker>();

        return services;
    }
}