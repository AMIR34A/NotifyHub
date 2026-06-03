using NotifyHub.Core.Contracts.Services;
using NotifyHub.Core.Domain.Exceptions;
using NotifyHub.Shared.Utility.Exceptions;
using NotifyHub.Shared.Utility.Results;

namespace NotifyHub.Infrastructure.Services.Sms;

public class SmsProvider(IEnumerable<ISmsService> smsProviders)
{
    private readonly IEnumerable<ISmsService> _smsProviders = smsProviders;

    public async Task Send(string receiver, string message)
    {
        foreach (var smsProvider in _smsProviders)
        {
            OperationResult result = await smsProvider.SendAsync(receiver, message);

            if (result.Succeed)
                return;
        }

        throw new ServiceException(Error.Unexpected());
    }
}