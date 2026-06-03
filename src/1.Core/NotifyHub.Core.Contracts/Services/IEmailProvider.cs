using NotifyHub.Shared.Utility.Results;

namespace NotifyHub.Core.Contracts.Services;

public interface IEmailProvider
{
    Task<IOperationResult> SendAsync(string receiver, string subject, string body);
}