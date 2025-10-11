using MediatR;

namespace Atria.Application.Features.Notifications.MarkAsRead;

public class MarkAllAsReadCommand : IRequest<Unit>
{
    public string UserId { get; init; } = string.Empty;
}