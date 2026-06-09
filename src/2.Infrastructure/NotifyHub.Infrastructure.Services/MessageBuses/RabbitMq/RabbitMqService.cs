using Microsoft.Extensions.Logging;
using NotifyHub.Core.BuildingBlocks.Events;
using NotifyHub.Core.Contracts.Services;
using RabbitMQ.Client;
using System.Text.RegularExpressions;

namespace NotifyHub.Infrastructure.Services.MessageBuses.RabbitMq;

internal sealed class RabbitMqService : IMessageBusService
{
    private readonly RabbitMqConnectionManager _connectionManager;
    private readonly IJsonSerializerService _jsonSerializerService;
    private readonly ILogger<RabbitMqService> _logger;

    public RabbitMqService(
        RabbitMqConnectionManager connectionManager,
        IJsonSerializerService jsonSerializerService,
        ILogger<RabbitMqService> logger)
    {
        _connectionManager = connectionManager;
        _jsonSerializerService = jsonSerializerService;
        _logger = logger;
    }

    public async Task Publish<TEvent>(TEvent @event)
        where TEvent : IDomainEvent
    {
        ArgumentNullException.ThrowIfNull(@event);

        var exchangeName = GetExchangeName<TEvent>();

        await using var channel = await CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: ExchangeType.Fanout,
            durable: true,
            autoDelete: false);

        await channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: string.Empty,
            mandatory: false,
            basicProperties: BuildProperties(),
            body: _jsonSerializerService.SerializeToUtf8Bytes(@event));

        _logger.LogDebug(
            "Published {Type} → exchange '{Exchange}'",
            typeof(TEvent).Name, exchangeName);
    }

    public async Task Send<TEvent>(TEvent @event)
        where TEvent : IDomainEvent
    {
        ArgumentNullException.ThrowIfNull(@event);

        var queueName = GetQueueName<TEvent>();

        await using var channel = await CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            mandatory: false,
            basicProperties: BuildProperties(),
            body: _jsonSerializerService.SerializeToUtf8Bytes(@event));

        _logger.LogDebug(
            "Sent {Type} → queue '{Queue}'",
            typeof(TEvent).Name, queueName);
    }

    private async Task<IChannel> CreateChannelAsync()
    {
        var connection = await _connectionManager.GetConnectionAsync();
        return await connection.CreateChannelAsync();
    }

    private static BasicProperties BuildProperties() => new()
    {
        Persistent = true,
        ContentType = "application/json",
        MessageId = Guid.NewGuid().ToString(),
        Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds()),
    };

    internal static string GetQueueName<T>() => ToKebabCase(typeof(T).Name);

    internal static string GetExchangeName<T>() => ToKebabCase(typeof(T).Namespace!);

    private static string ToKebabCase(string name) =>
        Regex.Replace(name, "(?<!^)([A-Z])", "-$1")
             .ToLowerInvariant();
}