using NotifyHub.Core.Contracts.Data.Notifications;

namespace NotifyHub.Infrastructure.Services;

public class DependencyHub
{
    public INotificationRepository NotificationRepository { get; }
    //public SmsService SmsService { get;};

    public DependencyHub(INotificationRepository notificationRepository)
    {
        NotificationRepository = notificationRepository;
    }
}