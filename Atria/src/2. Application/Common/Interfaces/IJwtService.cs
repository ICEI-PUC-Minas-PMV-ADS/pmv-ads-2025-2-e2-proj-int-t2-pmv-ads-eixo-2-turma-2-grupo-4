namespace Atria.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(string userId, string email, string role);
}