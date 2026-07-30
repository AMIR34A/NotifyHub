using MediatR;
using Microsoft.Extensions.Hosting;
using NotifyHub.Core.Contracts.Data.Notifications;
using NotifyHub.Core.Contracts.Factory;
using NotifyHub.Core.Domain.Notifications;
using NotifyHub.Core.RequestResponse.Notifications.Queries.GetNotificationsWithRetryStatus;
using Polly;

namespace NotifyHub.Infrastructure.Workers.Notifications;

internal class RetrySendingNotificationWorker(INotificationSenderFactory senderFactory,
    INotificationRepository notificationRepository,
    IMediator mediator,
    ResiliencePipeline pipeline) : BackgroundService
{
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var query = new GetNotificationsWithRetryStatusQuery();
            var notificationsWithRetryStatus = await mediator.Send(query, stoppingToken);

            await pipeline.ExecuteAsync(async (cancellationToken) =>
            {
                foreach (var notification in notificationsWithRetryStatus)
                {
                    var sender = senderFactory.CreateAsync(notification.Channel);
                    bool isSuccessful = await sender.SendAsync(notification.GetPreparedMessage(),
                        notification.Data,
                        cancellationToken);

                    if (isSuccessful)
                        notification.Sent();
                    else
                    {
                        if (notification.RetryCount == Notification.MaxRetryCount)
                        {
                            notification.Failed();
                            continue;
                        }

                        notification.IncreaseRetryCount();
                    }
                }
            }, stoppingToken);

            await notificationRepository.CommitAsync();
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }
}