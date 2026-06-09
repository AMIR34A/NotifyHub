using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notify.Infrastructure.Data;
using Notify.Infrastructure.Data.Repositories;
using NotifyHub.Core.Contracts.Data.Notifications;
using NotifyHub.Core.Contracts.Services;
using NotifyHub.Infrastructure.Services.Emails;
using NotifyHub.Infrastructure.Services.MessageBuses.RabbitMq;
using NotifyHub.Infrastructure.Services.SMSs;
using NotifyHub.Shared.Utility.AppSettings;

namespace NotifyHub.Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSettings>(configuration);

        services.AddDbContext<NotifyHubDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("NotifyHub"));
        });

        services.AddTransient<INotificationRepository, NotificationRepository>();

        services.AddSingleton<ISmsProvider, KavenegarProvider>();
        services.AddSingleton<ISmsProvider, SmsIrProvider>();
        services.AddSingleton<SmsService>();

        services.AddSingleton<IEmailProvider, LiaraEmailService>();
        services.AddSingleton<EmailService>();

        services.AddSingleton<RabbitMqConnectionManager>();
        services.AddScoped<IMessageBusService, RabbitMqService>();

        services.AddScoped<DependencyHub>();

        return services;
    }
}