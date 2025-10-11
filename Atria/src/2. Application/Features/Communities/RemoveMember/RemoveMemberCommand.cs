using MediatR;

namespace Atria.Application.Features.Communities.RemoveMember;

public class RemoveMemberCommand : IRequest<Unit>
{
    public int CommunityId { get; init; }
    public string UserId { get; init; } = string.Empty;
    public string RequesterId { get; init; } = string.Empty;
}