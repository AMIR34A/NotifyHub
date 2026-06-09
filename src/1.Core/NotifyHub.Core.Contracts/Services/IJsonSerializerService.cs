namespace NotifyHub.Core.Contracts.Services;

public interface IJsonSerializerService
{
    string Serialize<TInput>(TInput input);

    ReadOnlyMemory<byte> SerializeToUtf8Bytes<TInput>(TInput input);

    object? Deserialize(string input, Type type);

    TOutput? Deserialize<TOutput>(ReadOnlySpan<byte> span);

    TOutput? Deserialize<TOutput>(string input);
}