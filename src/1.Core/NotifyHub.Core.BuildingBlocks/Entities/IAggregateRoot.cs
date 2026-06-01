using NotifyHub.Core.BuildingBlocks.Events;

namespace NotifyHub.Core.BuildingBlocks.Entities;

public interface IAggregateRoot
{
    void ClearEvents();

    IReadOnlyCollection<IDomainEvent> Events { get; }
}