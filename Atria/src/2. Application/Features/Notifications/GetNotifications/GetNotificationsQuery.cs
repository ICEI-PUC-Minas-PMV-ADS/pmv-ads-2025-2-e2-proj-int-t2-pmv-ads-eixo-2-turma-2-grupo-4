using MediatR;
using System.Collections.Generic;

namespace Atria.Application.Features.Notifications.GetNotifications;

public class GetNotificationsQuery : IRequest<IEnumerable<NotificationDto>>
{
    public string UserId { get; init; } = string.Empty;
}