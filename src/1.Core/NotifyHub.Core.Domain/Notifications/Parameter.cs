using NotifyHub.Core.BuildingBlocks.Entities;

namespace NotifyHub.Core.Domain.Notifications;

public class Parameter : Entity<int>
{
    public int Order { get; private set; }

    public string Value { get; private set; } = default!;
}