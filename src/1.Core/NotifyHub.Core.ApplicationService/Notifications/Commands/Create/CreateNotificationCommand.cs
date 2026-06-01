using MediatR;
using NotifyHub.Core.Domain.Notifications;

namespace NotifyHub.Core.ApplicationService.Notifications.Commands.Create;

public sealed record CreateNotificationCommand(
    Channel Channel,
    Message Message,
    ICollection<Parameter> Parameters,
    string Data,
    string RequestedBy
) : IRequest;