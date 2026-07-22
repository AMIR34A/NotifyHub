using NotifyHub.Core.Domain.Notifications;

namespace NotifyHub.Core.Contracts.Factory;

public interface INotificationSenderFactory
{
    INotificationSender CreateAsync(Channel channel);
}