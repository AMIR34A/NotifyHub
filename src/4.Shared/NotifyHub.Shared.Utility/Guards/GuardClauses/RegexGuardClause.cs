using NotifyHub.Shared.Utility.Exceptions;
using System.Text.RegularExpressions;

namespace NotifyHub.Shared.Utility.Guards.GuardClauses;

public static class RegexGuardClause
{
    public static void Match(this Guard guard, string value, string pattern) =>
        guard.Match(value, pattern, new BaseException(Error.Validation()));

    public static void Match(this Guard guard, string value, string pattern, string message)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(message);
        guard.Match(value, pattern, new BaseException(Error.Validation(description: message)));
    }

    public static void Match<TException>(this Guard guard, string value, string pattern, TException exception)
        where TException : BaseException
    {
        ArgumentNullException.ThrowIfNull(exception);

        if (Regex.IsMatch(value, pattern))
            throw exception;
    }

    public static void NotMatch(this Guard guard, string value, string pattern) =>
        guard.NotMatch(value, pattern, new BaseException(Error.Validation()));

    public static void NotMatch(this Guard guard, string value, string pattern, string message)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(message);
        guard.NotMatch(value, pattern, new BaseException(Error.Validation(description: message)));
    }

    public static void NotMatch<TException>(this Guard guard, string value, string pattern, TException exception)
        where TException : BaseException
    {
        ArgumentNullException.ThrowIfNull(exception);

        if (!Regex.IsMatch(value, pattern))
            throw exception;
    }
}