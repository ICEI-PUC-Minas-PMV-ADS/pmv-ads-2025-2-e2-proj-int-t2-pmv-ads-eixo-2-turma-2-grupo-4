# Atria - Arquitetura e Fluxo do Projeto

Este documento descreve a arquitetura atual, principais fluxos e instruções de execução do projeto Atria para desenvolvedores.

## Visão Geral
- Projeto multi-camada: Domain, Application, Infrastructure, Api.
- Padrões usados: CQRS com MediatR, FluentValidation para validação de comandos, EF Core para persistência.
- Autenticação: JWT (serviço `IJwtService`).
- Banco: MySQL (migrations via EF Core).

## Principais Camadas
- `src/1. Domain`: Entidades e regras de negócio (models). Ex.: `Usuario`, `Comunidade`, `Material`.
- `src/2. Application`: Handlers, Commands/Queries, DTOs, contratos (IApplicationDbContext).
- `src/3. Infrastructure`: Implementação do DbContext (`AppDbContext`), configurações EF, serviços (ex.: `JwtService`).
- `src/4. Api`: Entrada HTTP (Controllers), configuração, Swagger, Dockerfile.

## Fluxos importantes

### Autenticação
- `AuthController` recebe credenciais e usa `LoginUserCommandHandler` para validar.
- `IJwtService.GenerateToken` cria token com claim `id` (IdUsuario) e `role` (TipoUsuario).
- Controllers protegem endpoints com `[Authorize]` e usam claim `id` para auditoria/ownership.

### Mensagens Privadas
- `SendMessageCommand` cria `MensagemPrivada` com `FkRemetente` obtido do claim do token.
- `GetMessagesQuery` busca mensagens por usuário.
- `MarkAsReadCommand` marca mensagem como lida (apenas destinatário).

### Usuários / Perfil
- Registro: `RegisterUserCommandHandler` — impede criação de `Moderador` via endpoint público.
- `UsersController` expõe `GET /api/users/me` e `PUT /api/users/{id}` (com validação ownership/admin).

### Comunidades e Membros
- `Comunidade` e `ComunidadeMembro` modelam membros com flags `IsAdmin`, `IsModAdmin`, `IsPending`.
- Endpoints: adicionar/remover/listar membros (`CommunitiesController`), convidar/aceitar (`InvitesController`).
- Promotions: `AdminController` / `PromoteUserCommand` para promover membros a admin/modadmin.

### Materiais
- Professores criam materiais (`CreateMaterialCommand`) — podem vincular material a comunidade.
- Avaliação: `ApproveMaterialCommand` — podem aprovar moderadores do sistema ou mod-admins da comunidade.

### Postagens e Comentários
- `CreatePostCommand` valida regras fórum geral vs comunidade.
- `CreateCommentCommand` valida membership para comentários em comunidade.

### Notificações
- `Notificacao` persistida; endpoints para listar e marcar como lida.
- Convites geram notificações para usuários convidados.

## Como rodar localmente (Docker)
Pré-requisitos: Docker e Docker Compose.

1. Arquivo `docker-compose.yml` contém serviços: `db` (MySQL), `adminer`, `api`.
2. Variáveis de ambiente apontam connection string para `db` (nome do serviço):
   - `ConnectionStrings__DefaultConnection: "Server=db;Port=3306;Database=atria;User=root;Password=example;..."`
3. Build e run:
   - docker compose up --build
4. Após o banco estar disponível, use `dotnet ef database update` via container ou local para aplicar migrações.

## Rodando localmente sem Docker
- Configure `src/4. Api/appsettings.Development.json` connection string.
- Executar `dotnet ef database update` para aplicar migrations.
- Rodar API: `dotnet run --project src/4. Api/Atria.Api.csproj`

## Testes
- Projeto de testes `tests/Atria.Application.Tests` usa `Microsoft.EntityFrameworkCore.InMemory`.
- Rodar: `dotnet test tests/Atria.Application.Tests/Atria.Application.Tests.csproj`

## Dockerfile atualizado
- `src/4. Api/Dockerfile` atualizado para copiar `src/1. Domain` projeto e permitir `dotnet restore` dentro do container.

## Regras de segurança / operações administrativas
- Moderadores do sistema: contas internas (não criáveis via API pública).
- Admins da comunidade: atribuíveis pelo criador ou por system moderator via `PromoteUserCommand`.

## Próximos passos recomendados
- Adicionar testes de integração para endpoints JWT-protegidos.
- Adicionar validações (FluentValidation) para todos comandos críticos.
- Implementar SignalR para notificações em tempo real.

---
Documentação inicial gerada automaticamente. Atualize conforme o projeto evolui.
