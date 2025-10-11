using MediatR;

namespace Atria.Application.Features.Notifications.MarkAsRead;

public class MarkNotificationAsReadCommand : IRequest<Unit>
{
    public int NotificationId { get; init; }
    public string RequesterId { get; init; } = string.Empty;
}