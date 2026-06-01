using NotifyHub.Core.BuildingBlocks.Events;

namespace NotifyHub.Core.BuildingBlocks.Entities;

public class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
    where TId : struct
{
    private readonly List<IDomainEvent> _events;
    protected AggregateRoot() => _events = [];

    IReadOnlyCollection<IDomainEvent> IAggregateRoot.Events => _events;

    public void ClearEvents() => _events.Clear();

    protected void AddEvent<TDomainEvent>(IDomainEvent @event) => _events.Add(@event);
}
