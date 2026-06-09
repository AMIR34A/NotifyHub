using Kavenegar;
using Microsoft.Extensions.Options;
using NotifyHub.Core.Contracts.Services;
using NotifyHub.Shared.Utility.AppSettings;
using NotifyHub.Shared.Utility.AppSettings.Sms;
using NotifyHub.Shared.Utility.Exceptions;
using NotifyHub.Shared.Utility.Results;

namespace NotifyHub.Infrastructure.Services.SMSs;

public class KavenegarProvider : ISmsProvider
{
    private readonly KavenegarOptions _kavenegarOptions;
    private readonly KavenegarApi _kavenegarApi;

    public KavenegarProvider(IOptions<AppSettings> options)
    {
        _kavenegarOptions = options.Value.SmsProviders.Kavenegar;
        _kavenegarApi = new KavenegarApi(_kavenegarOptions.ApiKey);
    }

    public async Task<IOperationResult> SendAsync(string receiver, string message, CancellationToken cancellationToken)
    {
        try
        {
            return await Task.Run(() =>
            {
                var result = _kavenegarApi.Send(_kavenegarOptions.Sender, receiver, message);

                if (result is not null && result.Status == 1)
                    return OperationResult.Succuss();

                return OperationResult.Fail(ErrorType.Unexpected, [$"Failed on {nameof(KavenegarProvider)} Service"]);

            }, cancellationToken).ConfigureAwait(false);

        }
        catch
        {
            return OperationResult.Fail(ErrorType.Unexpected, [$"Failed on {nameof(KavenegarProvider)} Service"]);
        }
    }

    public Task Inquiry()
    {
        throw new NotImplementedException();
    }
}