using MediatR;

namespace Atria.Application.Features.Communities.InviteMember;

public class InviteMemberCommand : IRequest<Unit>
{
    public int CommunityId { get; init; }
    public string TargetUserId { get; init; } = string.Empty;
    public string RequesterId { get; init; } = string.Empty;
}