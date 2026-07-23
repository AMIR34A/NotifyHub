using NotifyHub.Core.BuildingBlocks.ValueObjects;

namespace NotifyHub.Core.Domain.Notifications;

public class Parameter : ValueObject<Parameter>
{
    public int Order { get; private set; }

    public string Value { get; private set; } = default!;

    private Parameter() { }

    public Parameter(int order, string value)
    {
        Order = order;
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}