using MediatR;
using NotifyHub.Core.Contracts.Data.Notifications;
using NotifyHub.Core.Domain.Notifications;
using NotifyHub.Core.RequestResponse.Notifications.Queries.GetUnprocessedNotifications;

namespace NotifyHub.Core.ApplicationService.Notifications.Queries.GetUnprocessedNotifications;

public class GetUnprocessedNotificationsQueryHandler(INotificationRepository notificationRepository) : IRequestHandler<GetUnprocessedNotificationsQuery, IEnumerable<Notification>>
{
    public async Task<IEnumerable<Notification>> Handle(GetUnprocessedNotificationsQuery request, CancellationToken cancellationToken)
    {
        return await notificationRepository.GetUnprocessedNotifications(request.PageSize, cancellationToken);
    }
}