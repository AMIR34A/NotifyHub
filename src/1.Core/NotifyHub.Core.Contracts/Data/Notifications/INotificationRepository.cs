using NotifyHub.Core.Contracts.Data.Base;
using NotifyHub.Core.Domain.Notifications;

namespace NotifyHub.Core.Contracts.Data.Notifications;

public interface INotificationRepository : IBaseRepository<Notification, Guid>, IUnitOfWork
{
    Task<IEnumerable<Notification>> GetUnprocessedNotifications(int size, CancellationToken cancellationToken);

    Task<IEnumerable<Notification>> GetNotificationsWithRetryStatus(int size, CancellationToken cancellationToken);
}