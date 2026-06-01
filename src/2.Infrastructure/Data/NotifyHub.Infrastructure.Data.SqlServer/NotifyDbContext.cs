using Microsoft.EntityFrameworkCore;
using NotifyHub.Core.Domain.Notifications;
using NotifyHub.Infrastructure.Data.SqlServer.Base;

namespace Notify.Infrastructure.Data;

public class NotifyDbContext : BaseDbContext
{
    protected override string Schema => "Notify";

    public DbSet<Notification> Notifications { get; set; }

    public NotifyDbContext(DbContextOptions options) : base(options)
    {
    }
}