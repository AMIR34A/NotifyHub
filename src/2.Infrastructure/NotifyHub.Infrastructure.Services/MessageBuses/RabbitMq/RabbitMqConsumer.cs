using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotifyHub.Core.BuildingBlocks.Events;
using NotifyHub.Core.Contracts.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotifyHub.Infrastructure.Services.MessageBuses.RabbitMq;

public abstract class RabbitMqConsumer<TEvent> : BackgroundService
    where TEvent : IDomainEvent
{
    private readonly RabbitMqConnectionManager _connectionManager;
    private readonly IJsonSerializerService _jsonSerializerService;
    private readonly ILogger _logger;
    private IChannel? _channel;
    private string _activeQueue = string.Empty;

    protected RabbitMqConsumer(
        RabbitMqConnectionManager connectionManager,
        IJsonSerializerService jsonSerializerService,
        ILogger logger)
    {
        _connectionManager = connectionManager;
        _jsonSerializerService = jsonSerializerService;
        _logger = logger;
    }

    protected virtual bool IsSubscription => false;

    protected virtual ushort PrefetchCount => 10;

    protected virtual string QueueName => RabbitMqService.GetQueueName<TEvent>();

    protected virtual string ExchangeName => RabbitMqService.GetExchangeName<TEvent>();

    protected abstract Task HandleAsync(TEvent message, CancellationToken cancellationToken);

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var connection = await _connectionManager.GetConnectionAsync(cancellationToken);
        _channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await _channel.BasicQosAsync(
            prefetchSize: 0,
            prefetchCount: PrefetchCount,
            global: false,
            cancellationToken: cancellationToken);

        if (IsSubscription)
        {
            await _channel.ExchangeDeclareAsync(
                exchange: ExchangeName, type: ExchangeType.Fanout,
                durable: true, autoDelete: false, cancellationToken: cancellationToken);

            var result = await _channel.QueueDeclareAsync(
                queue: string.Empty, durable: false,
                exclusive: true, autoDelete: true,
                cancellationToken: cancellationToken);

            _activeQueue = result.QueueName;

            await _channel.QueueBindAsync(
                queue: _activeQueue, exchange: ExchangeName,
                routingKey: string.Empty, cancellationToken: cancellationToken);

            _logger.LogInformation(
                "[{Consumer}] Subscribed to exchange '{Exchange}' via queue '{Queue}'",
                GetType().Name, ExchangeName, _activeQueue);
        }
        else
        {
            await _channel.QueueDeclareAsync(
                queue: QueueName, durable: true,
                exclusive: false, autoDelete: false,
                cancellationToken: cancellationToken);

            _activeQueue = QueueName;

            _logger.LogInformation(
                "[{Consumer}] Listening on queue '{Queue}'",
                GetType().Name, _activeQueue);
        }

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel!);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            if (stoppingToken.IsCancellationRequested)
                return;

            TEvent? message = default;

            try
            {
                message = _jsonSerializerService.Deserialize<TEvent>(ea.Body.Span);

                if (message is null)
                {
                    _logger.LogWarning(
                        "[{Consumer}] Received null/undeserializable message — discarding.",
                        GetType().Name);

                    await _channel!.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
                    return;
                }

                await HandleAsync(message, stoppingToken);

                await _channel!.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "[{Consumer}] Failed to process {Type} — sending to dead letter.",
                    GetType().Name, typeof(TEvent).Name);

                await _channel!.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
            }
        };

        await _channel!.BasicConsumeAsync(_activeQueue, false, consumer);

        await Task.Delay(Timeout.Infinite, stoppingToken)
                  .ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
    }

    public override async Task StopAsync(CancellationToken ct)
    {
        await base.StopAsync(ct);

        if (_channel is not null)
        {
            await _channel.CloseAsync(ct);
            await _channel.DisposeAsync();
        }
    }
}