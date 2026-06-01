using NotifyHub.Core.BuildingBlocks.ValueObjects;
using NotifyHub.Core.Domain.Exceptions;
using NotifyHub.Shared.Utility.Exceptions;
using NotifyHub.Shared.Utility.Guards;
using NotifyHub.Shared.Utility.Guards.GuardClauses;

namespace NotifyHub.Core.Domain.Notifications;

public class Message : ValueObject<Message>
{
    public string Value { get; private set; } = default!;

    private Message() { }

    public Message(string value)
    {
        Guard.ThrowExceptionIf.MaximumLength(value,25, new DomainException(Error.Validation()));

        List<string> strings = new List<string>();

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}