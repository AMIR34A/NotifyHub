using NotifyHub.Core.BuildingBlocks.ValueObjects;
using NotifyHub.Core.Domain.Exceptions;
using NotifyHub.Shared.Utility.Exceptions;
using NotifyHub.Shared.Utility.Guards;
using NotifyHub.Shared.Utility.Guards.GuardClauses;

namespace NotifyHub.Core.Domain.Notifications;

public class Parameter : ValueObject<Parameter>
{
    public int Order { get; private set; }

    public string Value { get; private set; } = default!;

    private Parameter() { }

    public Parameter(int order, string value)
    {
        Guard.ThrowExceptionIf.LessThan(order, 0, new DomainException(Error.Validation()));
        Guard.ThrowExceptionIf.Empty(value, new DomainException(Error.Validation()));

        Order = order;
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}