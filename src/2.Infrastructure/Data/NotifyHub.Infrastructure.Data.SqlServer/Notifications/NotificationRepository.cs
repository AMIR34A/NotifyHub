using Microsoft.EntityFrameworkCore;
using NotifyHub.Core.Contracts.Data.Notifications;
using NotifyHub.Core.Domain.Notifications;
using NotifyHub.Infrastructure.Data.SqlServer.Base;

namespace Notify.Infrastructure.Data.Repositories;

public class NotificationRepository : BaseRepository<Notification, NotifyHubDbContext, Guid>, INotificationRepository
{
    public NotificationRepository(NotifyHubDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Notification>> GetUnprocessedNotifications(int size, CancellationToken cancellationToken) => await _dbContext.Notifications
        .Where(n => n.Status == Status.InQueue)
        .Take(size)
        .Include(n => n.Message)
        .Include(n => n.Parameters)
        .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Notification>> GetNotificationsWithRetryStatus(int size, CancellationToken cancellationToken) => await _dbContext.Notifications
        .Where(n => n.Status == Status.Retry && n.RetryCount < Notification.MaxRetryCount)
        .Take(size)
        .Include(n => n.Message)
        .Include(n => n.Parameters)
        .ToListAsync(cancellationToken);
}