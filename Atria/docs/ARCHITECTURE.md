# Atria - Arquitetura e Fluxo do Projeto

Este documento descreve a arquitetura atual, principais fluxos e instru��es de execu��o do projeto Atria para desenvolvedores.

## Vis�o Geral
- Projeto multi-camada: Domain, Application, Infrastructure, Api.
- Padr�es usados: CQRS com MediatR, FluentValidation para valida��o de comandos, EF Core para persist�ncia.
- Autentica��o: JWT (servi�o `IJwtService`).
- Banco: MySQL (migrations via EF Core).

## Principais Camadas
- `src/1. Domain`: Entidades e regras de neg�cio (models). Ex.: `Usuario`, `Comunidade`, `Material`.
- `src/2. Application`: Handlers, Commands/Queries, DTOs, contratos (IApplicationDbContext).
- `src/3. Infrastructure`: Implementa��o do DbContext (`AppDbContext`), configura��es EF, servi�os (ex.: `JwtService`).
- `src/4. Api`: Entrada HTTP (Controllers), configura��o, Swagger, Dockerfile.

## Fluxos importantes

### Autentica��o
- `AuthController` recebe credenciais e usa `LoginUserCommandHandler` para validar.
- `IJwtService.GenerateToken` cria token com claim `id` (IdUsuario) e `role` (TipoUsuario).
- Controllers protegem endpoints com `[Authorize]` e usam claim `id` para auditoria/ownership.

### Mensagens Privadas
- `SendMessageCommand` cria `MensagemPrivada` com `FkRemetente` obtido do claim do token.
- `GetMessagesQuery` busca mensagens por usu�rio.
- `MarkAsReadCommand` marca mensagem como lida (apenas destinat�rio).

### Usu�rios / Perfil
- Registro: `RegisterUserCommandHandler` � impede cria��o de `Moderador` via endpoint p�blico.
- `UsersController` exp�e `GET /api/users/me` e `PUT /api/users/{id}` (com valida��o ownership/admin).

### Comunidades e Membros
- `Comunidade` e `ComunidadeMembro` modelam membros com flags `IsAdmin`, `IsModAdmin`, `IsPending`.
- Endpoints: adicionar/remover/listar membros (`CommunitiesController`), convidar/aceitar (`InvitesController`).
- Promotions: `AdminController` / `PromoteUserCommand` para promover membros a admin/modadmin.

### Materiais
- Professores criam materiais (`CreateMaterialCommand`) � podem vincular material a comunidade.
- Avalia��o: `ApproveMaterialCommand` � podem aprovar moderadores do sistema ou mod-admins da comunidade.

### Postagens e Coment�rios
- `CreatePostCommand` valida regras f�rum geral vs comunidade.
- `CreateCommentCommand` valida membership para coment�rios em comunidade.

### Notifica��es
- `Notificacao` persistida; endpoints para listar e marcar como lida.
- Convites geram notifica��es para usu�rios convidados.

## Como rodar localmente (Docker)
Pr�-requisitos: Docker e Docker Compose.

1. Arquivo `docker-compose.yml` cont�m servi�os: `db` (MySQL), `adminer`, `api`.
2. Vari�veis de ambiente apontam connection string para `db` (nome do servi�o):
   - `ConnectionStrings__DefaultConnection: "Server=db;Port=3306;Database=atria;User=root;Password=example;..."`
3. Build e run:
   - docker compose up --build
4. Ap�s o banco estar dispon�vel, use `dotnet ef database update` via container ou local para aplicar migra��es.

## Rodando localmente sem Docker
- Configure `src/4. Api/appsettings.Development.json` connection string.
- Executar `dotnet ef database update` para aplicar migrations.
- Rodar API: `dotnet run --project src/4. Api/Atria.Api.csproj`

## Testes
- Projeto de testes `tests/Atria.Application.Tests` usa `Microsoft.EntityFrameworkCore.InMemory`.
- Rodar: `dotnet test tests/Atria.Application.Tests/Atria.Application.Tests.csproj`

## Dockerfile atualizado
- `src/4. Api/Dockerfile` atualizado para copiar `src/1. Domain` projeto e permitir `dotnet restore` dentro do container.

## Regras de seguran�a / opera��es administrativas
- Moderadores do sistema: contas internas (n�o cri�veis via API p�blica).
- Admins da comunidade: atribu�veis pelo criador ou por system moderator via `PromoteUserCommand`.

## Pr�ximos passos recomendados
- Adicionar testes de integra��o para endpoints JWT-protegidos.
- Adicionar valida��es (FluentValidation) para todos comandos cr�ticos.
- Implementar SignalR para notifica��es em tempo real.

---
Documenta��o inicial gerada automaticamente. Atualize conforme o projeto evolui.
