using NotifyHub.Core.Domain.Notifications;

namespace NotifyHub.Core.Contracts.Factory;

public interface INotificationSender
{
    Channel Channel { get; }

    Task SendAsync(string payload);
}