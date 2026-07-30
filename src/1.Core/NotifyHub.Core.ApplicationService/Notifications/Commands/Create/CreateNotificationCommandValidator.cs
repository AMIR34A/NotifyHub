using FluentValidation;
using NotifyHub.Core.RequestResponse.Notifications.Commands.Create;

namespace NotifyHub.Core.ApplicationService.Notifications.Commands.Create;

public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
{
    public CreateNotificationCommandValidator()
    {
        RuleFor(property => property.Channel)
            .NotNull()
            .NotEmpty();

        RuleFor(property => property.Message)
            .NotNull()
            .Must(message =>
            {
                return message?.Value?.Length > 1500;
            });

        RuleFor(property => property.Data)
            .NotNull()
            .NotEmpty();

        RuleFor(property => property.RequestedBy)
            .NotNull()
            .NotEmpty();
    }
}