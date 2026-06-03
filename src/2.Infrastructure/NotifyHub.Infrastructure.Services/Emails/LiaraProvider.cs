using FluentEmail.Core;
using Microsoft.Extensions.Options;
using NotifyHub.Core.Contracts.Services;
using NotifyHub.Shared.Utility.AppSettings;
using NotifyHub.Shared.Utility.AppSettings.Email;
using NotifyHub.Shared.Utility.Exceptions;
using NotifyHub.Shared.Utility.Results;

namespace NotifyHub.Infrastructure.Services.Emails;

public class LiaraEmailService(IOptions<AppSettings> options) : IEmailProvider
{
    private readonly BaseEmailOptions _emailOptions = options.Value.EmailProviders.Base;

    public async Task<IOperationResult> SendAsync(string receiver, string subject, string body)
    {
        try
        {
            await Email.From(_emailOptions.DefaultEmailAddress)
                       .To(receiver)
                       .Subject(subject)
                       .PlaintextAlternativeBody(body)
                       .SendAsync();

            return OperationResult.Succuss();
        }
        catch
        {
            return OperationResult.Fail(ErrorType.Unexpected, [$"Failed on {nameof(LiaraEmailService)} Service"]);
        }
    }
}