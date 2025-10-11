Atria
=====

Resumo
------
Reposit�rio da aplica��o Atria � plataforma de comunidade/compartilhamento acad�mico.
Arquitetura em camadas: `Domain`, `Application`, `Infrastructure`, `Api`.
Para documenta��o detalhada veja `docs/ARCHITECTURE.md` e `docs/DEVELOPMENT-SETUP.md`.

Links r�pidos
-------------
- Arquitetura e fluxo: `docs/ARCHITECTURE.md`
- Setup e execu��o: `docs/DEVELOPMENT-SETUP.md`

Quick start (Docker)
--------------------
1. Subir containers:

   docker compose up --build

2. A API ficar� dispon�vel em `http://localhost:5000`.
3. Acesse Adminer em `http://localhost:8080` (usu�rio `root`, senha conforme `docker-compose.yml`).

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

Contribui��o
------------
- Branches: use `feature/<nome>` ou `fix/<nome>` para PRs.
- Inclua testes para altera��es de comportamento e atualize migrations quando necess�rio.

Observa��es
-----------
- Moderadores do sistema n�o podem ser criados via endpoint p�blico � use seed/migration ou cria��o manual em ambiente controlado.
- Para d�vidas sobre fluxo ou regras de neg�cio, consulte `docs/ARCHITECTURE.md`.

