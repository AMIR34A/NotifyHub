using NotifyHub.Core.BuildingBlocks.ValueObjects;
using NotifyHub.Core.Domain.Exceptions;
using NotifyHub.Shared.Utility.Exceptions;
using NotifyHub.Shared.Utility.Guards;
using NotifyHub.Shared.Utility.Guards.GuardClauses;
using System.Text.RegularExpressions;

namespace NotifyHub.Core.Domain.Notifications;

public class Message : ValueObject<Message>
{
    public string Value { get; private set; } = default!;

    public bool NeedsParameters => Regex.IsMatch(Value, @"\{\s*(\d+)\s*\}");

    private Message() { }

    public Message(string value)
    {
        Guard.ThrowExceptionIf.MaximumLength(value, 25, new DomainException(Error.Validation()));
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}