using MediatR;

namespace Atria.Application.Features.Admin.PromoteUser;

public class PromoteUserCommand : IRequest<Unit>
{
    public string TargetUserId { get; init; } = string.Empty;
    public string RequesterId { get; init; } = string.Empty;
    public bool MakeModerator { get; init; } = false; // not used: moderators are internal
    public bool MakeAdminInCommunity { get; init; } = false;
    public int? CommunityId { get; init; }
    public bool MakeModAdmin { get; init; } = false;
}