namespace Atria.Application.Features.Communities.ListMembers;

public class MemberDto
{
    public string UserId { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public bool IsModAdmin { get; set; }
    public string JoinedAt { get; set; } = string.Empty;
}