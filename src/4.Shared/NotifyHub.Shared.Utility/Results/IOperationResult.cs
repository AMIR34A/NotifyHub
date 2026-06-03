using NotifyHub.Shared.Utility.Exceptions;

namespace NotifyHub.Shared.Utility.Results;

public interface IOperationResult
{
    string this[int index] { get; }

    public bool Succeed { get; }

    ErrorType ErrorType { get; }

    IEnumerable<string> Messages { get; }
}