using NotifyHub.Core.Contracts.Services;
using NotifyHub.Core.Domain.Exceptions;
using NotifyHub.Shared.Utility.Exceptions;

namespace NotifyHub.Infrastructure.Services.SMSs;

public class SmsService(IEnumerable<ISmsProvider> smsProviders)
{
    private readonly IEnumerable<ISmsProvider> _smsProviders = smsProviders;

    public async Task Send(string receiver, string message)
    {
        foreach (var smsProvider in _smsProviders)
        {
            var result = await smsProvider.SendAsync(receiver, message);

            if (result.Succeed)
                return;
        }

        throw new ServiceException(Error.Unexpected());
    }
}