using MediatR;

namespace Atria.Application.Features.Posts.CreatePost;

public class CreatePostCommand : IRequest<int>
{
    public string Title { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public int? CommunityId { get; init; }
    public bool NoForumGeral { get; init; }
    public string RequesterId { get; init; } = string.Empty;
}