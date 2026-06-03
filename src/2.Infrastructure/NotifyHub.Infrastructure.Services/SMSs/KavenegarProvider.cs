using Kavenegar;
using Microsoft.Extensions.Options;
using NotifyHub.Core.Contracts.Services;
using NotifyHub.Shared.Utility.AppSettings;
using NotifyHub.Shared.Utility.AppSettings.Sms;
using NotifyHub.Shared.Utility.Exceptions;
using NotifyHub.Shared.Utility.Results;

namespace NotifyHub.Infrastructure.Services.SMSs;

public class KavenegarProvider(IOptions<AppSettings> options) : ISmsService
{
    private readonly KavenegarOptions _kavenegarOptions = options.Value.SmsProviders.Kavenegar;

    public async Task<IOperationResult> SendAsync(string receiver, string message)
    {
        try
        {
            KavenegarApi kavenegarApi = new KavenegarApi(_kavenegarOptions.ApiKey);

            return await Task.Run(() =>
            {
                var result = kavenegarApi.Send(_kavenegarOptions.Sender, receiver, message);

                if (result is not null && result.Status == 1)
                    return OperationResult.Succuss();

                return OperationResult.Fail(ErrorType.Unexpected, [$"Failed on {nameof(KavenegarProvider)} Service"]);

            }).ConfigureAwait(false);

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