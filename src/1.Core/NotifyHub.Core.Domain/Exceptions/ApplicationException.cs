using NotifyHub.Shared.Utility.Exceptions;

namespace NotifyHub.Core.Domain.Exceptions
{
    public class ApplicationException : BaseException
    {
        public ApplicationException(Error error) : base(error)
        {
        }
    }
}