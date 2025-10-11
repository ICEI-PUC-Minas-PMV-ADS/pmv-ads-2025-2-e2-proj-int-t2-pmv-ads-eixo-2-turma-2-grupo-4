using System;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Common.Interfaces;
using Atria.Domain.Entities.UserContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atria.Application.Features.Users.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly ILogger<LoginUserCommandHandler> _logger;

    public LoginUserCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IJwtService jwtService,
        ILogger<LoginUserCommandHandler> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha))
        {
            throw new ArgumentException("Email e senha são obrigatórios");
        }

        var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (user == null)
        {
            throw new ArgumentException("Usuário ou senha inválidos");
        }

        if (!_passwordHasher.VerifyPassword(user.SenhaHash, request.Senha))
        {
            throw new ArgumentException("Usuário ou senha inválidos");
        }

        var token = _jwtService.GenerateToken(user.IdUsuario, user.Email, user.TipoUsuario.ToString());
        _logger.LogInformation("Usuário {UserId} autenticado com sucesso", user.IdUsuario);
        return token;
    }
}