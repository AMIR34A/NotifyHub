namespace NotifyHub.Core.Contracts.Services;

public interface IJsonSerializerService
{
    string Serialize<TInput>(TInput input);

    object? Deserialize(string input, Type type);

    TOutput? Deserialize<TOutput>(string input);
}