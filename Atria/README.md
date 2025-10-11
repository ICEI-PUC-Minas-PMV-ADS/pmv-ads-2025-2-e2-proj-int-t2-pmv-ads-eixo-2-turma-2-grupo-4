Atria
=====

Resumo
------
Repositório da aplicação Atria — plataforma de comunidade/compartilhamento acadêmico.
Arquitetura em camadas: `Domain`, `Application`, `Infrastructure`, `Api`.
Para documentação detalhada veja `docs/ARCHITECTURE.md` e `docs/DEVELOPMENT-SETUP.md`.

Links rápidos
-------------
- Arquitetura e fluxo: `docs/ARCHITECTURE.md`
- Setup e execução: `docs/DEVELOPMENT-SETUP.md`

Quick start (Docker)
--------------------
1. Subir containers:

   docker compose up --build

2. A API ficará disponível em `http://localhost:5000`.
3. Acesse Adminer em `http://localhost:8080` (usuário `root`, senha conforme `docker-compose.yml`).

Executar localmente (sem Docker)
--------------------------------
1. Configure `src/4. Api/appsettings.Development.json` com sua connection string MySQL.
2. Restaurar pacotes:

   dotnet restore

3. Aplicar migrations:

   dotnet ef database update --project src/3. Infrastructure/Atria.Infrastructure.csproj --startup-project src/4. Api/Atria.Api.csproj --context AppDbContext

4. Executar API:

   dotnet run --project src/4. Api/Atria.Api.csproj

Testes
------
- Projeto de testes: `tests/Atria.Application.Tests`
- Executar:

  dotnet test tests/Atria.Application.Tests/Atria.Application.Tests.csproj

Contribuição
------------
- Branches: use `feature/<nome>` ou `fix/<nome>` para PRs.
- Inclua testes para alterações de comportamento e atualize migrations quando necessário.

Observações
-----------
- Moderadores do sistema não podem ser criados via endpoint público — use seed/migration ou criação manual em ambiente controlado.
- Para dúvidas sobre fluxo ou regras de negócio, consulte `docs/ARCHITECTURE.md`.

