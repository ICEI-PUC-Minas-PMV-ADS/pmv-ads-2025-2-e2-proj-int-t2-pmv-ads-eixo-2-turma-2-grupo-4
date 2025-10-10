using System;
using System.Threading;
using System.Threading.Tasks;
using Atria.Application.Common.Interfaces;
using Atria.Domain.Entities.UserContext;
using Atria.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace Atria.Application.Features.Users.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<RegisterUserCommandHandler> _logger;

    public RegisterUserCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        ILogger<RegisterUserCommandHandler> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verificar se o email já existe
            var existingUser = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (existingUser != null)
            {
                throw new ValidationException("Email já cadastrado");
            }

            // Se for professor, validar matrícula
            if (request.TipoUsuario == TipoUsuario.Professor)
            {
                if (string.IsNullOrEmpty(request.Matricula))
                {
                    throw new ValidationException("Matrícula é obrigatória para professores");
                }

                var existingMatricula = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Matricula == request.Matricula, cancellationToken);

                if (existingMatricula != null)
                {
                    throw new ValidationException("Matrícula já cadastrada");
                }
            }

            var usuario = new Usuario
            {
                IdUsuario = Guid.NewGuid().ToString(),
                Nome = request.Nome,
                Email = request.Email,
                SenhaHash = _passwordHasher.HashPassword(request.Senha),
                TipoUsuario = request.TipoUsuario,
                DataCadastro = DateTime.UtcNow,
                Matricula = request.Matricula,
                AreaAtuacao = request.AreaAtuacao
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Usuário {UserId} registrado com sucesso", usuario.IdUsuario);

            return usuario.IdUsuario;
        }
        catch (ValidationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar usuário");
            throw new ApplicationException("Erro ao processar o registro do usuário", ex);
        }
    }
}