using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotifyHub.Core.ApplicationService.Notifications.Events;
using NotifyHub.Infrastructure.Services;

namespace NotifyHub.Core.ApplicationService;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureApplicationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(configure =>
        {
            configure.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services.AddHostedService<NotificationReceivedHandler>();

        return services;
    }
}