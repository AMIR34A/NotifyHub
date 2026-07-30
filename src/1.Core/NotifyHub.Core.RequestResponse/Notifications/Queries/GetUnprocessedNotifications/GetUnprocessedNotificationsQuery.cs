using MediatR;
using NotifyHub.Core.BuildingBlocks.Queries;
using NotifyHub.Core.Domain.Notifications;

namespace NotifyHub.Core.RequestResponse.Notifications.Queries.GetUnprocessedNotifications;

public record GetUnprocessedNotificationsQuery : QueryBase, IRequest<IEnumerable<Notification>>;