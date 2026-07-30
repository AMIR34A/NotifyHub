namespace NotifyHub.Core.Domain.Notifications;

public enum Status : byte
{
    InQueue = 0,
    Sent = 1,
    Delivered = 2,
    Retry = 3,
    Failed = 4
}