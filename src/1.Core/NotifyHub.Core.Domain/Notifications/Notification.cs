using NotifyHub.Core.BuildingBlocks.Entities;
using NotifyHub.Core.Domain.Exceptions;
using NotifyHub.Shared.Utility.Exceptions;
using NotifyHub.Shared.Utility.Guards;
using NotifyHub.Shared.Utility.Guards.GuardClauses;

namespace NotifyHub.Core.Domain.Notifications;

public class Notification : AggregateRoot<long>
{
    public Channel Channel { get; private set; }

    public Status Status { get; private set; }

    public Message Message { get; private set; } = default!;

    public ICollection<Parameter>? Parameters { get; private set; }

    public string Data { get; private set; } = default!;

    public string RequestedBy { get; private set; } = default!;

    public DateTime RequestedAt { get; private set; }

    public string? TraceId { get; private set; }

    public string? ProviderName { get; private set; }

    public static Notification Create(Channel channel,
        Message message,
        ICollection<Parameter> parameters,
        string data,
        string requestedBy)
    {
        Guard.ThrowExceptionIf.Empty(message, new DomainException(Error.Validation()));
        Guard.ThrowExceptionIf.Empty(data, new DomainException(Error.Validation()));
        Guard.ThrowExceptionIf.MaximumLength(requestedBy, 25, new DomainException(Error.Validation()));

        if (message.NeedsParameters)
            Guard.ThrowExceptionIf.Empty(parameters, new DomainException(Error.Validation()));

        return new Notification
        {
            Channel = channel,
            Status = Status.InQueue,
            Message = message,
            Parameters = parameters,
            Data = data,
            RequestedBy = requestedBy,
            RequestedAt = DateTime.Now
        };
    }
}