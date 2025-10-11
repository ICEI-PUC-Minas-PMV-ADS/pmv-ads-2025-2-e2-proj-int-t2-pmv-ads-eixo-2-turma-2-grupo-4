using MediatR;
using System.Collections.Generic;

namespace Atria.Application.Features.Communities.ListMembers;

public class ListMembersQuery : IRequest<IEnumerable<MemberDto>>
{
    public int CommunityId { get; init; }
}