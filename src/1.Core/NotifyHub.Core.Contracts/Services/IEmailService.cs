using NotifyHub.Shared.Utility.Results;

namespace NotifyHub.Core.Contracts.Services;

public interface IEmailService
{
    Task<IOperationResult> SendAsync(string receiver, string subject, string body);
}