using NotifyHub.Shared.Utility.Results;

namespace NotifyHub.Core.Contracts.Services;

public interface ISmsService
{
    Task<OperationResult> SendAsync(string receiver, string message);

    Task Inquiry();
}