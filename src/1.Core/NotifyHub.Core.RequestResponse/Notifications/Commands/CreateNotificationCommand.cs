using FluentValidation;
using MediatR;
using NotifyHub.Core.Domain.Notifications;

namespace NotifyHub.Core.RequestResponse.Notifications.Commands;

public sealed record CreateNotificationCommand(
    Channel Channel,
    string Template,
    ICollection<Parameter> Parameters,
    string Data,
    string RequestedBy
) : IRequest;

public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
{
    public CreateNotificationCommandValidator()
    {
        RuleFor(property => property.Channel)
            .NotNull()
            .NotEmpty();

        RuleFor(property => property.Template)
            .NotNull()
            .NotEmpty();

        RuleFor(property => property.Data)
            .NotNull()
            .NotEmpty();

        RuleFor(property => property.RequestedBy)
            .NotNull()
            .NotEmpty();
    }
}