using MediatR;
using NotifyHub.Core.BuildingBlocks.Queries;
using NotifyHub.Core.Domain.Notifications;

namespace NotifyHub.Core.RequestResponse.Notifications.Queries.GetNotificationsWithRetryStatus;

public record GetNotificationsWithRetryStatusQuery : QueryBase, IRequest<IEnumerable<Notification>>;
