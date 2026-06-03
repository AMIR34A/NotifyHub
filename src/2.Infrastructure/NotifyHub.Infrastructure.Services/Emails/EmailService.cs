using NotifyHub.Core.Contracts.Services;
using NotifyHub.Core.Domain.Exceptions;
using NotifyHub.Shared.Utility.Exceptions;

namespace NotifyHub.Infrastructure.Services.Emails
{
    public class EmailService(IEnumerable<IEmailProvider> emailProviders)
    {
        private readonly IEnumerable<IEmailProvider> _emailProviders = emailProviders;

        public async Task Send(string receiver, string subject, string body)
        {
            foreach (var emailProvider in _emailProviders)
            {
                var result = await emailProvider.SendAsync(receiver, subject, body);

                if (result.Succeed)
                    return;
            }

            throw new ServiceException(Error.Unexpected());
        }
    }
}