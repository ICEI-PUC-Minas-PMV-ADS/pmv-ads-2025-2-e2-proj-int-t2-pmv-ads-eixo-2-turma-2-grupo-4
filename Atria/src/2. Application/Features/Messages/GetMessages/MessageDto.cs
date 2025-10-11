using System;

namespace Atria.Application.Features.Messages.GetMessages;

public class MessageDto
{
    public string Id { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
    public bool Read { get; set; }
    public string SenderId { get; set; } = string.Empty;
    public string RecipientId { get; set; } = string.Empty;
}