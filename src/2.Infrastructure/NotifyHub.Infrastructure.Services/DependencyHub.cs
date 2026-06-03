using NotifyHub.Core.Contracts.Data.Notifications;
using NotifyHub.Infrastructure.Services.Emails;
using NotifyHub.Infrastructure.Services.SMSs;

namespace NotifyHub.Infrastructure.Services;

public class DependencyHub
{
    public INotificationRepository NotificationRepository { get; }

    public SmsService SmsService { get; }

    public EmailService EmailService { get; }

    public DependencyHub(INotificationRepository notificationRepository, SmsService smsService, EmailService emailService)
    {
        NotificationRepository = notificationRepository;
        SmsService = smsService;
        EmailService = emailService;
    }
}