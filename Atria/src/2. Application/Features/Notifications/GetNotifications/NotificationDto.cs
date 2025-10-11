using System;

namespace Atria.Application.Features.Notifications.GetNotifications;

public class NotificationDto
{
    public int Id { get; set; }
    public string Conteudo { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public bool Lida { get; set; }
}