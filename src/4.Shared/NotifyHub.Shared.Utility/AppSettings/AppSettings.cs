using NotifyHub.Shared.Utility.AppSettings.Email;
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
    public required RabbitMqOptions RabbitMQ { get; set; }
}

public sealed class SmsProviders
{
    public required KavenegarOptions Kavenegar { get; set; }

    public required SmsIrOptions SmsIr { get; set; }
}

public sealed class EmailProviders
{
    public required BaseEmailOptions Base { get; set; }
}
