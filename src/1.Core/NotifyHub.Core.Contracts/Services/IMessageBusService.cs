using NotifyHub.Core.BuildingBlocks.Events;

namespace NotifyHub.Core.Contracts.Services;

public interface IMessageBusService
{
    Task Publish<TEvent>(TEvent input) where TEvent : IDomainEvent;

    Task Send<TEvent>(TEvent input) where TEvent : IDomainEvent;
}