using MediatR;

namespace Atria.Application.Features.Communities.AcceptInvite;

public class AcceptInviteCommand : IRequest<Unit>
{
    public int CommunityId { get; init; }
    public string UserId { get; init; } = string.Empty;
}