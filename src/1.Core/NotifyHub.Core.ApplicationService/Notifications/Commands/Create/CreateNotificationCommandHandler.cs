using MediatR;
using NotifyHub.Core.Contracts.Data.Notifications;
using NotifyHub.Core.Domain.Notifications;

namespace NotifyHub.Core.ApplicationService.Notifications.Commands.Create;

public class CreateNotificationCommandHandler(INotificationRepository _notificationRepository) : IRequestHandler<CreateNotificationCommand>
{
    public async Task Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        Notification notification = Notification.Create(request.Channel,
            request.Message,
            request.Parameters,
            request.Data,
            request.RequestedBy);

        await _notificationRepository.InsertAsync(notification, cancellationToken);
        await _notificationRepository.CommitAsync();
    }
}