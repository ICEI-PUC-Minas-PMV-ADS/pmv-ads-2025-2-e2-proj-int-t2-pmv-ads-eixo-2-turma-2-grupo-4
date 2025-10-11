using MediatR;

namespace Atria.Application.Features.Messages.SendMessage;

public class SendMessageCommand : IRequest<string>
{
    // SenderId will be set by the controller using JWT claim; client should not send this value.
    public string SenderId { get; set; } = string.Empty;
    public string RecipientId { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
}