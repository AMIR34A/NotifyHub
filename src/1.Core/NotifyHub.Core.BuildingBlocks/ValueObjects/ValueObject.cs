namespace NotifyHub.Core.BuildingBlocks.ValueObjects;

public abstract class ValueObject<T> where T : ValueObject<T>
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj) => obj is ValueObject<T> valueObject &&
                                                GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());

    public override int GetHashCode() => GetEqualityComponents()
                                        .Select(x => x?.GetHashCode() ?? 0)
                                        .Aggregate((x, y) => x ^ y);
}