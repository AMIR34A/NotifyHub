using NotifyHub.Shared.Utility.AppSettings.Sms;

namespace NotifyHub.Shared.Utility.AppSettings;

public class AppSettings
{
    public required MessageBrokers MessageBrokers { get; set; }

    public required SmsProviders SmsProviders { get; set; }

    public required EmailProviders EmailProviders { get; set; }
}

public sealed class MessageBrokers
{
    public required RabbitMQ RabbitMQ { get; set; }
}

public sealed class SmsProviders
{
    public required KavenegarOptions Kavenegar { get; set; }

    public required SmsIrOptions SmsIr { get; set; }
}

public sealed class EmailProviders
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
