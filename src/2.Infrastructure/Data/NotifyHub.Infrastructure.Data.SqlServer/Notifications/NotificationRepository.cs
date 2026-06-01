using NotifyHub.Core.Contracts.Data.Notifications;
using NotifyHub.Core.Domain.Notifications;
using NotifyHub.Infrastructure.Data.SqlServer.Base;

namespace Notify.Infrastructure.Data.Repositories;

public class NotificationRepository : BaseRepository<Notification, NotifyDbContext, long>, INotificationRepository
{
    public NotificationRepository(NotifyDbContext dbContext) : base(dbContext)
    {
    }
}