using NotifyHub.Core.Contracts.Factory;
using NotifyHub.Core.Domain.Exceptions;
using NotifyHub.Core.Domain.Notifications;
using NotifyHub.Shared.Utility.Exceptions;

namespace NotifyHub.Infrastructure.Services.Factory;

public class NotificationSenderFactory : INotificationSenderFactory
{
    private readonly Dictionary<Channel, INotificationSender> _senders;

    public NotificationSenderFactory(IEnumerable<INotificationSender> senders)
    {
        _senders = senders.ToDictionary(s => s.Channel, s => s);
    }

    public INotificationSender CreateAsync(Channel channel)
    {
        if (_senders is null || _senders.Count == 0)
            throw new ServiceException(Error.Failure());

        if (_senders.TryGetValue(channel, out INotificationSender? sender))
            return sender;

        throw new ServiceException(Error.NotFound());
    }
}