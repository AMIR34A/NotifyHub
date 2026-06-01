using NotifyHub.Shared.Utility.Exceptions;

namespace NotifyHub.Shared.Utility.ResultPattern;

public record OperationResult : IOperationResult
{
    private readonly List<string> _messages = new();

    public string this[int index]
    {
        get
        {
            if (_messages.Count > index)
                return _messages[index];

            throw new IndexOutOfRangeException();
        }
    }

    public bool Succeed { get; }

    public ErrorType ErrorType { get; }

    public IEnumerable<string> Messages => _messages;

    protected OperationResult() => _messages ??= [];

    protected OperationResult(bool succeed, ErrorType errorType = default, IEnumerable<string> messages = default!)
    {
        Succeed = succeed;
        if (!succeed)
        {
            ErrorType = errorType;
            _messages.AddRange(messages);
        }
    }

    public void AddMessage(string error) => _messages.Add(error);

    public void AddMessages(IEnumerable<string> errors) => _messages.AddRange(errors);

    public void ClearMessages() => _messages.Clear();

    public static OperationResult Succuss() => new OperationResult(true);

    public static OperationResult Fail(ErrorType errorType, IEnumerable<string> messages) => new OperationResult(false, errorType, messages);
}

public record OperationResult<TResult> : OperationResult
    where TResult : notnull
{
    public TResult Result { get; }

    protected OperationResult(bool succeed, ErrorType errorType = default, IEnumerable<string> messages = default!, TResult result = default!)
        : base(succeed, errorType, messages) => Result = result;

    public static OperationResult<TResult> Succuss(TResult result) => new OperationResult<TResult>(true, result: result);
}