using NotifyHub.Core.Contracts.Factory;
using NotifyHub.Core.Contracts.Services;
using NotifyHub.Core.Domain.Exceptions;
using NotifyHub.Core.Domain.Notifications;
using NotifyHub.Shared.Utility.Exceptions;
using NotifyHub.Shared.Utility.Guards;
using NotifyHub.Shared.Utility.Guards.GuardClauses;

namespace NotifyHub.Infrastructure.Services.Emails;

public class EmailService(IEnumerable<IEmailProvider> emailProviders, IJsonSerializerService jsonSerializer) : INotificationSender
{
    public Channel Channel => Channel.Email;

    public async Task<bool> SendAsync(string message, string payload, CancellationToken cancellationToken)
    {
        Guard.ThrowExceptionIf.Empty(payload, new ServiceException(Error.Failure()));
        Guard.ThrowExceptionIf.Empty(message, new ServiceException(Error.Failure()));

        EmailPayload? emailPayload = jsonSerializer.Deserialize<EmailPayload>(payload);

        Guard.ThrowExceptionIf.Null(emailPayload, new ServiceException(Error.Failure()));
        Guard.ThrowExceptionIf.Empty(emailPayload!.Receiver, new ServiceException(Error.Failure()));
        Guard.ThrowExceptionIf.Empty(emailPayload.Subject, new ServiceException(Error.Failure()));

        foreach (var emailProvider in emailProviders)
        {
            var result = await emailProvider.SendAsync(emailPayload.Receiver, emailPayload.Subject, message, cancellationToken);

            if (result.Succeed)
                return true;
        }

        return false;
    }
}

internal sealed record EmailPayload(string Receiver, string Subject);
