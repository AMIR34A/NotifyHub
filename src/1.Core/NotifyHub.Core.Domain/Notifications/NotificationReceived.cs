using NotifyHub.Core.BuildingBlocks.Events;

namespace NotifyHub.Core.Domain.Notifications;

public sealed record NotificationReceived(Channel Channel,
    Message Message,
    ICollection<Parameter> Parameters,
    string Data,
    string RequestedBy) : IDomainEvent;