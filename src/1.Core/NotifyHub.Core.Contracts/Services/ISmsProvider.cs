using NotifyHub.Shared.Utility.Results;

namespace NotifyHub.Core.Contracts.Services;

public interface ISmsProvider
{
    Task<IOperationResult> SendAsync(string receiver, string message, CancellationToken cancellationToken);

    Task Inquiry();
}