using MediatR;

namespace Atria.Application.Features.Messages.GetMessages;

public class GetMessagesQuery : IRequest<IEnumerable<MessageDto>>
{
    public string UserId { get; init; } = string.Empty;
}