# Atria - Guia de Desenvolvimento (Setup)

Este documento descreve como configurar o ambiente para desenvolvimento e contribui��es.

## Pr�-requisitos
- Git
- .NET 9 SDK
- Docker & Docker Compose (opcional, recomendado)
- MySQL (se preferir rodar localmente sem Docker)

## Clonar reposit�rio
```bash
git clone https://github.com/ICEI-PUC-Minas-PMV-ADS/pmv-ads-2025-2-e2-proj-int-t2-pmv-ads-eixo-2-turma-2-grupo-4.git
cd Atria
```

## Rodar com Docker (recomendado)
1. Ajuste se necess�rio `docker-compose.yml` (senha do MySQL, portas).
2. Suba containers:
```bash
docker compose up --build
```
3. A API estar� exposta em `http://localhost:5000` por padr�o.
4. Aplicar migrations (op��es):
   - Executar `dotnet ef database update` localmente apontando para a database do container, ou
   - Entrar no container `api`/`dotnet ef database update`.

## Rodar local sem Docker
1. Configure `src/4. Api/appsettings.Development.json` com sua connection string MySQL.
2. Restaurar pacotes:
```bash
dotnet restore
```
3. Aplicar migrations:
```bash
dotnet ef database update --project src/3. Infrastructure/Atria.Infrastructure.csproj --startup-project src/4. Api/Atria.Api.csproj --context AppDbContext
```
4. Rodar API:
```bash
dotnet run --project src/4. Api/Atria.Api.csproj
```

## Testes
- Projeto de testes unit�rios localizado em `tests/Atria.Application.Tests`.
- Rodar testes:
```bash
dotnet test tests/Atria.Application.Tests/Atria.Application.Tests.csproj
```

## Dockerfile notas
- `src/4. Api/Dockerfile` � multi-stage (build + runtime). O Dockerfile copia os projetos Domain, Application, Infrastructure e API para permitir `dotnet restore` no container.

## Vari�veis de ambiente �teis
- `ConnectionStrings__DefaultConnection` � string de conex�o para MySQL
- `Jwt__Key`, `Jwt__Issuer`, `Jwt__ExpiryMinutes` � configura��o do JWT

## Fluxo de PR / Contribui��o
- Crie branch a partir de `main` com o padr�o `feature/<nome>` ou `fix/<nome>`.
- Abra PR descrevendo altera��o e testes adicionados.
- Mantenha Migration files fora de conflitos (evite reescrever migrations existentes).

## Observa��es
- Moderadores do sistema n�o podem ser criados via API p�blica � use seed ou adm direto no DB para cria��o em ambientes controlados.
- Use `Adminer` (dispon�vel no docker-compose na porta 8080) para inspecionar o banco durante desenvolvimento.

---
Atualize este guia se mudar infraestrutura ou processos de desenvolvimento.
