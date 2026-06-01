using NotifyHub.Core.Contracts.Data.Base;
using NotifyHub.Core.Domain.Notifications;

namespace NotifyHub.Core.Contracts.Data.Notifications;

public interface INotificationRepository : IBaseRepository<Notification, long>, IUnitOfWork
{
}