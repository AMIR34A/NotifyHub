using NotifyHub.Shared.Utility.Exceptions;

namespace NotifyHub.Core.Domain.Exceptions;

public class ServiceException : BaseException
{
    public ServiceException(Error error) : base(error)
    {
    }
}