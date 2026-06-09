using MediatR;
using Microsoft.Extensions.Logging;
using NotifyHub.Core.ApplicationService.Notifications.Commands.Create;
using NotifyHub.Core.Contracts.Services;
using NotifyHub.Core.Domain.Notifications;
using NotifyHub.Infrastructure.Services.MessageBuses.RabbitMq;

namespace NotifyHub.Core.ApplicationService.Notifications.Events
{
    internal class NotificationReceivedHandler : RabbitMqConsumer<NotificationReceived>
    {
        private readonly IMediator _mediator;

        public NotificationReceivedHandler(IMediator mediator,
            RabbitMqConnectionManager connectionManager,
            IJsonSerializerService jsonSerializerService,
            ILogger<NotificationReceivedHandler> logger) : base(connectionManager, jsonSerializerService, logger)
        {
            _mediator = mediator;
        }

        protected async override Task HandleAsync(NotificationReceived message, CancellationToken cancellationToken)
        {
            var command = new CreateNotificationCommand(message.Channel,
                message.Message,
                message.Parameters,
                message.Data,
                message.RequestedBy);

            await _mediator.Send(command, cancellationToken);
        }
    }
}