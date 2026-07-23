using System.Text;

namespace NotifyHub.Shared.Utility.Exceptions;

public readonly record struct Error
{
    public string Title { get; init; } = default!;

    public string Description { get; init; } = default!;

    public string Code { get; init; } = default!;

    public ErrorType Type { get; init; }

    public string[]? Parameters { get; init; }

    public Error(string title, string description, string code, ErrorType type, string[]? parameters)
    {
        Title = title;
        Description = ToString();
        Code = code;
        Type = type;
        Parameters = parameters;
    }

    public static Error Failure(
        string title = "خطا در انجام عملیات",
        string description = "خطایی رخ داده است.",
        string code = "general.failure",
        ErrorType type = ErrorType.Failure,
        string[]? parameters = null) => new Error(title, description, code, type, parameters);

    public static Error Validation(
        string title = "اعتبارسنجی",
        string description = "خطایی در اعتبار سنجی رخ داده است.",
        string code = "general.validation",
        ErrorType type = ErrorType.Validation,
        string[]? parameters = null) => new Error(title, description, code, type, parameters);

    public static Error Unexpected(
        string title = "خطای غیرمنتظره",
        string description = "یک خطای غیرمنتظره رخ داده است.",
        string code = "general.unexpected",
        ErrorType type = ErrorType.Unexpected,
        string[]? parameters = null) => new Error(title, description, code, type, parameters);

    public static Error NotFound(
        string title = "یافت نشد",
        string description = "منبع مورد نظر یافت نشد.",
        string code = "general.not_found",
        ErrorType type = ErrorType.NotFound,
        string[]? parameters = null) => new Error(title, description, code, type, parameters);

    public override string ToString()
    {
        if (Parameters is null || Parameters.Length == 0)
            return Description;

        StringBuilder errorMessage = new();
        errorMessage.AppendFormat(Description, Parameters);

        return errorMessage.ToString();
    }
}