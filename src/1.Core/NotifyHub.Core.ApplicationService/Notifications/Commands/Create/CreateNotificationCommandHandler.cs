using MediatR;
using NotifyHub.Core.Domain.Notifications;
using NotifyHub.Infrastructure.Services;

namespace NotifyHub.Core.ApplicationService.Notifications.Commands.Create;

public class CreateNotificationCommandHandler(DependencyHub dependencyHub) : IRequestHandler<CreateNotificationCommand>
{
    private readonly DependencyHub _dependencyHub = dependencyHub;

    public async Task Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        Notification notification = Notification.Create(request.Channel,
            request.Message,
            request.Parameters,
            request.Data,
            request.RequestedBy);

        await _dependencyHub.NotificationRepository.InsertAsync(notification, cancellationToken);
        _dependencyHub.NotificationRepository.Commit();
    }
}