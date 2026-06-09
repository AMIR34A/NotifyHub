using IPE.SmsIrClient;
using Microsoft.Extensions.Options;
using NotifyHub.Core.Contracts.Services;
using NotifyHub.Shared.Utility.AppSettings;
using NotifyHub.Shared.Utility.AppSettings.Sms;
using NotifyHub.Shared.Utility.Exceptions;
using NotifyHub.Shared.Utility.Results;

namespace NotifyHub.Infrastructure.Services.SMSs;

public class SmsIrProvider : ISmsProvider
{
    private readonly SmsIrOptions _smsIrOptions;
    private readonly SmsIr _smsIr;

    public SmsIrProvider(IOptions<AppSettings> options)
    {
        _smsIrOptions = options.Value.SmsProviders.SmsIr;
        _smsIr = new SmsIr(_smsIrOptions.ApiKey);
    }

    public async Task<IOperationResult> SendAsync(string receiver, string message, CancellationToken cancellationToken)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await _smsIr.BulkSendAsync(_smsIrOptions.LineNumber, message, [receiver])
                                                        .ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();

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