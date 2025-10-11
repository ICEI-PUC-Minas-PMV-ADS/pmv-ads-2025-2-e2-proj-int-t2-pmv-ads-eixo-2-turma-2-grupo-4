using MediatR;
using System.Collections.Generic;
using Atria.Application.Features.Notifications.GetNotifications;

namespace Atria.Application.Features.Notifications.GetUnread;

public class GetUnreadNotificationsQuery : IRequest<IEnumerable<NotificationDto>>
{
    public string UserId { get; init; } = string.Empty;
}