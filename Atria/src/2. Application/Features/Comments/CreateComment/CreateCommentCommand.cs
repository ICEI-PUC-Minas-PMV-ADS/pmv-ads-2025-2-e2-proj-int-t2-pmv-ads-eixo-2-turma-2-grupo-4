using MediatR;

namespace Atria.Application.Features.Comments.CreateComment;

public class CreateCommentCommand : IRequest<int>
{
    public int PostId { get; init; }
    public string Content { get; init; } = string.Empty;
    public string RequesterId { get; init; } = string.Empty;
}