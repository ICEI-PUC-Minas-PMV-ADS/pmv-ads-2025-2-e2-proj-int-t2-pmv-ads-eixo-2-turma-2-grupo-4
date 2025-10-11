using MediatR;

namespace Atria.Application.Features.Communities.AddMember;

public class AddMemberCommand : IRequest<Unit>
{
    public int CommunityId { get; init; }
    public string UserId { get; init; } = string.Empty;
    public string RequesterId { get; init; } = string.Empty;
    public bool IsAdmin { get; init; } = false;
    public bool IsModAdmin { get; init; } = false;
}