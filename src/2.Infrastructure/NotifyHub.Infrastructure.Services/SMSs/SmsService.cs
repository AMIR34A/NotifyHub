using NotifyHub.Core.Contracts.Factory;
using NotifyHub.Core.Contracts.Services;
using NotifyHub.Core.Domain.Exceptions;
using NotifyHub.Core.Domain.Notifications;
using NotifyHub.Shared.Utility.Exceptions;
using Polly;

namespace NotifyHub.Infrastructure.Services.SMSs;

public class SmsService(IEnumerable<ISmsProvider> smsProviders, IJsonSerializerService jsonSerializer, ResiliencePipeline pipeline) : INotificationSender
{
    public Channel Channel => Channel.Sms;

    public async Task SendAsync(string payload)
    {
        SmsPayload? smsPayload = jsonSerializer.Deserialize<SmsPayload>(payload);

        if (smsPayload is null)
            throw new ServiceException(Error.Failure());

        await pipeline.ExecuteAsync(async cancellationToken =>
        {
            foreach (var smsProvider in smsProviders)
            {
                var result = await smsProvider.SendAsync(smsPayload.Receiver, smsPayload.Message, cancellationToken);

                if (result.Succeed)
                    return;
            }

            throw new ServiceException(Error.Unexpected());
        });
    }
}

internal sealed record SmsPayload(string Receiver, string Message);