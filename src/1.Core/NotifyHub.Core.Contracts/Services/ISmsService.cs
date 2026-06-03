using NotifyHub.Shared.Utility.Results;

namespace NotifyHub.Core.Contracts.Services;

public interface ISmsService
{
    Task<IOperationResult> SendAsync(string receiver, string message);

    Task Inquiry();
}