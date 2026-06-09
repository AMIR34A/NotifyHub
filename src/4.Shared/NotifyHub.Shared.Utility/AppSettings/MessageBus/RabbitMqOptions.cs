namespace NotifyHub.Shared.Utility.AppSettings;

public sealed class RabbitMqOptions
{
    public required string HostName { get; set; }

    public required int Port { get; set; }

    public required string UserName { get; set; }

    public required string Password { get; set; }

    public required string VirtualHost { get; set; }

    public required ushort ConsumerDispatchConcurrency { get; set; }
}