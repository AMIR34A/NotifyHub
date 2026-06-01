using NotifyHub.Core.Contracts.Services;
using RabbitMQ.Client;

namespace NotifyHub.Infrastructure.Services.MessageBus;

public class MessageBusService : IMessageBusService
{
    private readonly IChannel _channel;

    public MessageBusService()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "admin",
            Password = "admin",
            VirtualHost = "/",
            AutomaticRecoveryEnabled = true, // reconnect on failure
        };
        using var connection = factory.CreateConnectionAsync();
        using var channel = connection.Result.CreateChannelAsync();
    }

    public Task Publish<TInput>(TInput input)
    {
        throw new NotImplementedException();
    }

    public Task Send<TInput>(TInput input)
    {
        throw new NotImplementedException();
    }
}