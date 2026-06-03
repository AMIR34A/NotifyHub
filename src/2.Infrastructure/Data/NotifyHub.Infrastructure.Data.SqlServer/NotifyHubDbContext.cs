using Microsoft.EntityFrameworkCore;
using NotifyHub.Core.Domain.Notifications;
using NotifyHub.Infrastructure.Data.SqlServer.Base;

namespace Notify.Infrastructure.Data;

public class NotifyHubDbContext : BaseDbContext
{
    protected override string Schema => "NotifyHub";

    public DbSet<Notification> Notifications { get; set; }

    public NotifyHubDbContext(DbContextOptions options) : base(options)
    {
    }
}