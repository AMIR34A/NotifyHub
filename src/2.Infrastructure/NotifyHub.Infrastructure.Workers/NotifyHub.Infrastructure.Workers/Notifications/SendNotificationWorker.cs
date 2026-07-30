using MediatR;
using Microsoft.Extensions.Hosting;
using NotifyHub.Core.Contracts.Data.Notifications;
using NotifyHub.Core.Contracts.Factory;
using NotifyHub.Core.RequestResponse.Notifications.Queries.GetUnprocessedNotifications;

namespace NotifyHub.Infrastructure.Workers.Notifications;

internal class SendNotificationWorker(INotificationSenderFactory senderFactory,
    INotificationRepository notificationRepository,
    IMediator mediator) : BackgroundService
{
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var query = new GetUnprocessedNotificationsQuery();
            var unprocessedNotifications = await mediator.Send(query, stoppingToken);

            foreach (var notification in unprocessedNotifications)
            {
                var sender = senderFactory.CreateAsync(notification.Channel);
                bool isSuccessful = await sender.SendAsync(notification.GetPreparedMessage(),
                    notification.Data,
                    stoppingToken);

                if (isSuccessful)
                    notification.Sent();
                else
                    notification.Retry();
            }

            await notificationRepository.CommitAsync();
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }
}