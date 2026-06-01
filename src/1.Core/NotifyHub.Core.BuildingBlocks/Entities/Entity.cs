namespace NotifyHub.Core.BuildingBlocks.Entities;

public abstract class Entity<TId> where TId : struct
{
    public TId Id { get; protected set; }

    protected Entity() { }

    #region Equality
    public override bool Equals(object? obj) => obj is Entity<TId> entity && Id.Equals(entity.Id);

    public override int GetHashCode() => HashCode.Combine(GetType(), Id);
    #endregion
}