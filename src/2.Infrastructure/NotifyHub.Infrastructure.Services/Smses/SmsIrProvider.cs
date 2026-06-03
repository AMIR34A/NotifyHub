using IPE.SmsIrClient;
using Microsoft.Extensions.Options;
using NotifyHub.Core.Contracts.Services;
using NotifyHub.Shared.Utility.AppSettings;
using NotifyHub.Shared.Utility.AppSettings.Sms;
using NotifyHub.Shared.Utility.Exceptions;
using NotifyHub.Shared.Utility.Results;

namespace NotifyHub.Infrastructure.Services.Smses;

public class SmsIrProvider(IOptions<AppSettings> options) : ISmsService
{
    private readonly SmsIrOptions _smsIrOptions = options.Value.SmsProviders.SmsIr;

    public async Task<IOperationResult> SendAsync(string receiver, string message)
    {
        try
        {
            SmsIr smsIr = new SmsIr(_smsIrOptions.ApiKey);
            var result = await smsIr.BulkSendAsync(_smsIrOptions.LineNumber, message, [receiver])
                                                       .ConfigureAwait(false);

            if (result is not null && result.Status == 1)
                return OperationResult.Succuss();

            return OperationResult.Fail(ErrorType.Unexpected, [$"Failed on {nameof(SmsIrProvider)} Service"]);
        }
        catch
        {
            return OperationResult.Fail(ErrorType.Unexpected, [$"Failed on {nameof(SmsIrProvider)} Service"]);
        }
    }

    public Task Inquiry()
    {
        throw new NotImplementedException();
    }
}