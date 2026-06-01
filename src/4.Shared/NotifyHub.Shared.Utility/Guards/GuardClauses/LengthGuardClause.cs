using NotifyHub.Shared.Utility.Exceptions;
using System.Collections;

namespace NotifyHub.Shared.Utility.Guards.GuardClauses;

public static class LengthGuardClause
{
    public static void Length(this Guard guard, string value, int length) =>
        guard.Length(value, length, new BaseException(Error.Validation()));

    public static void Length(this Guard guard, string value, int length, string message)
    {
        ArgumentNullException.ThrowIfNull(message);
        guard.Length(value, length, new BaseException(Error.Validation(description: message)));
    }

    public static void Length<TException>(this Guard guard, string value, int length, TException exception)
        where TException : BaseException
    {
        ArgumentNullException.ThrowIfNull(exception);

        if (value is null || value.Length != length)
            throw exception;
    }

    public static void Length(this Guard guard, ICollection value, int length) =>
        guard.Length(value, length, new BaseException(Error.Validation()));

    public static void Length(this Guard guard, ICollection value, int length, string message)
    {
        ArgumentNullException.ThrowIfNull(message);
        guard.Length(value, length, new BaseException(Error.Validation(description: message)));
    }

    public static void Length<TException>(this Guard guard, ICollection value, int length, TException exception)
        where TException : BaseException
    {
        ArgumentNullException.ThrowIfNull(exception);

        if (value is null || value.Count != length)
            throw exception;
    }
}