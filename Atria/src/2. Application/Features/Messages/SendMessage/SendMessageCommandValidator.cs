using FluentValidation;

namespace Atria.Application.Features.Messages.SendMessage;

public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(x => x.RecipientId).NotEmpty().WithMessage("RecipientId is required");
        RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required").MaximumLength(2000);
    }
}