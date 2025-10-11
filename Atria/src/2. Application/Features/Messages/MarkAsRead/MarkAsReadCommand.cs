using MediatR;

namespace Atria.Application.Features.Messages.MarkAsRead;

public class MarkAsReadCommand : IRequest<Unit>
{
    public string MessageId { get; init; } = string.Empty;
    public string RequesterId { get; init; } = string.Empty;
}