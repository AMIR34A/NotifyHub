using Microsoft.Extensions.Hosting;
using NotifyHub.Core.Contracts.Factory;
using NotifyHub.Core.Domain.Notifications;

namespace NotifyHub.Infrastructure.Workers.Notification;

public class SendNotificationWorker(INotificationSenderFactory senderFactory) : BackgroundService
{
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var sender = senderFactory.CreateAsync(Channel.Sms);
            await sender.SendAsync("");
        }
    }
}
