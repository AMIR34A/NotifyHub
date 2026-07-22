using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotifyHub.Infrastructure.Workers.Notification;

namespace NotifyHub.Infrastructure.Workers;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureWorkers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<SendNotificationWorker>();

        return services;
    }
}