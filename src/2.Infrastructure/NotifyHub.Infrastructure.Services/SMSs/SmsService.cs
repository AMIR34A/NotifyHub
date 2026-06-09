using NotifyHub.Core.Contracts.Services;
using NotifyHub.Core.Domain.Exceptions;
using NotifyHub.Shared.Utility.Exceptions;
using Polly;

namespace NotifyHub.Infrastructure.Services.SMSs;

public class SmsService(IEnumerable<ISmsProvider> smsProviders, ResiliencePipeline pipeline)
{
    private readonly IEnumerable<ISmsProvider> _smsProviders = smsProviders;

    public async Task Send(string receiver, string message)
    {
        await pipeline.ExecuteAsync(async cancellationToken =>
        {
            foreach (var smsProvider in _smsProviders)
            {
                var result = await smsProvider.SendAsync(receiver, message, cancellationToken);

                if (result.Succeed)
                    return;
            }

            throw new ServiceException(Error.Unexpected());
        });
    }
}