using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotifyHub.Shared.Utility.AppSettings;
using RabbitMQ.Client;

namespace NotifyHub.Infrastructure.Services.MessageBuses.RabbitMq;

public sealed class RabbitMqConnectionManager : IAsyncDisposable
{
    private readonly ConnectionFactory _factory;
    private readonly ILogger<RabbitMqConnectionManager> _logger;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private IConnection? _connection;

    public RabbitMqConnectionManager(
        IOptions<RabbitMqOptions> options,
        ILogger<RabbitMqConnectionManager> logger)
    {
        _logger = logger;
        var opt = options.Value;

        _factory = new ConnectionFactory
        {
            HostName = opt.HostName,
            Port = opt.Port,
            UserName = opt.UserName,
            Password = opt.Password,
            VirtualHost = opt.VirtualHost,
            AutomaticRecoveryEnabled = true,
            ConsumerDispatchConcurrency = opt.ConsumerDispatchConcurrency,
        };
    }

    public async Task<IConnection> GetConnectionAsync(CancellationToken ct = default)
    {
        if (_connection is { IsOpen: true })
            return _connection;

        await _lock.WaitAsync(ct);
        try
        {
            if (_connection is { IsOpen: true })
                return _connection;

            _logger.LogInformation(
                "Opening RabbitMQ connection → {Host}:{Port}",
                _factory.HostName, _factory.Port);

            _connection = await _factory.CreateConnectionAsync(ct);
            return _connection;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }

        _lock.Dispose();
    }
}