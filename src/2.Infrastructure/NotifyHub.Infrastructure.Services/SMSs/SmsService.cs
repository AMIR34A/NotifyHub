using NotifyHub.Core.Contracts.Factory;
using NotifyHub.Core.Contracts.Services;
using NotifyHub.Core.Domain.Exceptions;
using NotifyHub.Core.Domain.Notifications;
using NotifyHub.Shared.Utility.Exceptions;
using NotifyHub.Shared.Utility.Guards;
using NotifyHub.Shared.Utility.Guards.GuardClauses;

namespace NotifyHub.Infrastructure.Services.SMSs;

public class SmsService(IEnumerable<ISmsProvider> smsProviders,
    IJsonSerializerService jsonSerializer) : INotificationSender
{
    public Channel Channel => Channel.Sms;

    public async Task<bool> SendAsync(string message, string payload, CancellationToken cancellationToken)
    {
        Guard.ThrowExceptionIf.Empty(payload, new ServiceException(Error.Failure()));
        Guard.ThrowExceptionIf.Empty(message, new ServiceException(Error.Failure()));

        SmsPayload? smsPayload = jsonSerializer.Deserialize<SmsPayload>(payload);

        Guard.ThrowExceptionIf.Null(smsPayload, new ServiceException(Error.Failure()));
        Guard.ThrowExceptionIf.Empty(smsPayload!.Receiver, new ServiceException(Error.Failure()));

        foreach (var smsProvider in smsProviders)
        {
            var result = await smsProvider.SendAsync(smsPayload.Receiver, message, cancellationToken);

            if (result.Succeed)
                return true;
        }

        return false;
    }
}

internal sealed record SmsPayload(string Receiver);