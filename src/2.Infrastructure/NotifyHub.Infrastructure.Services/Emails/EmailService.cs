using NotifyHub.Core.Contracts.Services;
using NotifyHub.Core.Domain.Exceptions;
using NotifyHub.Shared.Utility.Exceptions;

namespace NotifyHub.Infrastructure.Services.Emails;

public class EmailService(IEnumerable<IEmailProvider> emailProviders, IJsonSerializerService jsonSerializer)
{
    public async Task SendAsync(string payload)
    {
        EmailPayload? emailPayload = jsonSerializer.Deserialize<EmailPayload>(payload);

        if (emailPayload is null)
            throw new ServiceException(Error.Failure());

        foreach (var emailProvider in emailProviders)
        {
            var result = await emailProvider.SendAsync(emailPayload.Receiver, emailPayload.Subject, emailPayload.Body);

            if (result.Succeed)
                return;
        }

        throw new ServiceException(Error.Unexpected());
    }
}

internal sealed record EmailPayload(string Receiver, string Subject, string Body);
