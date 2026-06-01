namespace NotifyHub.Shared.Utility.AppSettings;

public class AppSettings
{
    public required MessageBrokers MessageBrokers { get; set; }
}

public sealed class MessageBrokers
{
    public required RabbitMQ RabbitMQ { get; set; }
}

public sealed class Emails
{
    public required Liara Liara { get; set; }
}

public sealed class Liara
{
    public required string Url { get; set; }

    public required string AccessKey { get; set; }

    public required string SecretKey { get; set; }

    public required string BucketName { get; set; }
}

