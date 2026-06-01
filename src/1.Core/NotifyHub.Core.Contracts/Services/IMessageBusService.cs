namespace NotifyHub.Core.Contracts.Services;

public interface IMessageBusService
{
    Task Publish<TInput>(TInput input);

    Task Send<TInput>(TInput input);
}