using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notify.Infrastructure.Data;
using Notify.Infrastructure.Data.Repositories;
using NotifyHub.Core.Contracts.Data.Notifications;
using NotifyHub.Core.Contracts.Factory;
using NotifyHub.Core.Contracts.Services;
using NotifyHub.Infrastructure.Services.Emails;
using NotifyHub.Infrastructure.Services.Factory;
using NotifyHub.Infrastructure.Services.MessageBuses.RabbitMq;
using NotifyHub.Infrastructure.Services.SMSs;
using NotifyHub.Shared.Utility.AppSettings;
using Polly;
using Polly.Retry;

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
        services.AddSingleton<INotificationSender, SmsService>();

        services.AddSingleton<IEmailProvider, LiaraEmailService>();
        services.AddSingleton<EmailService>();

        services.AddSingleton<INotificationSenderFactory, NotificationSenderFactory>();

        services.AddSingleton<RabbitMqConnectionManager>();
        services.AddScoped<IMessageBusService, RabbitMqService>();

        services.AddResiliencePipeline("sms-pipeline", builder =>
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

        services.AddScoped<DependencyHub>();

        return services;
    }
}