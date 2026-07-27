using MediatR;
using NotifyHub.Core.Contracts.Data.Notifications;
using NotifyHub.Core.Domain.Notifications;
using NotifyHub.Shared.Utility.Exceptions;
using ApplicationException = NotifyHub.Core.Domain.Exceptions.ApplicationException;

namespace NotifyHub.Core.ApplicationService.Notifications.Commands.Create;

public class CreateNotificationCommandHandler(INotificationRepository notificationRepository) : IRequestHandler<CreateNotificationCommand>
{
    public async Task Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        if (await notificationRepository.ExistsAsync(n => n.Id == request.Id))
            throw new ApplicationException(Error.Failure());

        Notification notification = Notification.Create(request.Id, request.Channel,
            request.Message,
            request.Parameters,
            request.Data,
            request.RequestedBy);

        notificationRepository.Insert(notification);
        await notificationRepository.CommitAsync(cancellationToken);
    }
}