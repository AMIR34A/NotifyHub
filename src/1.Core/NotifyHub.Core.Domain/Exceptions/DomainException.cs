using NotifyHub.Shared.Utility.Exceptions;

namespace NotifyHub.Core.Domain.Exceptions;

public class DomainException : BaseException
{
    public DomainException(Error error) : base(error)
    {
    }
}