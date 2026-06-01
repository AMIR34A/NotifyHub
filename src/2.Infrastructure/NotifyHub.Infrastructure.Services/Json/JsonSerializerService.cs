using NotifyHub.Core.Contracts.Services;
using System.Text.Json;

namespace NotifyHub.Infrastructure.Services.Json;

public class JsonSerializerService : IJsonSerializerService
{
    public object? Deserialize(string input, Type type) => JsonSerializer.Deserialize(input, type);

    public TOutput? Deserialize<TOutput>(string input) => JsonSerializer.Deserialize<TOutput>(input);

    public string Serialize<TInput>(TInput input) => JsonSerializer.Serialize(input);
}