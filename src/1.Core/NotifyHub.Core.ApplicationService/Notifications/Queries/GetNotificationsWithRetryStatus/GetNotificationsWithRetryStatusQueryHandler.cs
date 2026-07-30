using MediatR;
using NotifyHub.Core.Contracts.Data.Notifications;
using NotifyHub.Core.Domain.Notifications;
using NotifyHub.Core.RequestResponse.Notifications.Queries.GetUnprocessedNotifications;

namespace NotifyHub.Core.ApplicationService.Notifications.Queries.GetNotificationsWithRetryStatus;

public class GetNotificationsWithRetryStatusQueryHandler(INotificationRepository notificationRepository) : IRequestHandler<GetUnprocessedNotificationsQuery, IEnumerable<Notification>>
{
    public async Task<IEnumerable<Notification>> Handle(GetUnprocessedNotificationsQuery request, CancellationToken cancellationToken)
    {
        return await notificationRepository.GetNotificationsWithRetryStatus(request.PageSize, cancellationToken);
    }
}