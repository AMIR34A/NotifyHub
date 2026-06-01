namespace NotifyHub.Core.Contracts.Services;

public interface ISmsService
{
    Task SendAsync();

    Task Inquiry();
}