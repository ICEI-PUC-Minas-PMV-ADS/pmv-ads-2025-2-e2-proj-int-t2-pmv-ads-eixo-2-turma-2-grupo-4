# Atria - Como rodar com Docker

Instru��es r�pidas para rodar o projeto com Docker Compose, acessar a API e o Adminer (interface web do MySQL).

Pr�-requisitos
- Docker (Docker Desktop ou engine) instalado e rodando
- (Opcional) dotnet 9 SDK para executar localmente sem containers

Rodando os containers
1. Na raiz do reposit�rio execute:
   `docker compose up --build -d`

2. Verifique logs/sa�de:
   - Logs da API: `docker compose logs -f api`
   - Logs do DB: `docker compose logs -f db`
   - Status dos containers: `docker compose ps`

Mapeamentos e endpoints
- API: http://localhost:5000 (Swagger dispon�vel em `/swagger` no ambiente Development)
- Adminer (UI do banco): http://localhost:8080
- MySQL no host (mapeado): porta `3307` -> container `3306` (use quando conectar externamente)

Credenciais do MySQL (apenas para desenvolvimento)
- Host (dentro do container): `db`
- Port (dentro do container): `3306`
- Host (externo / no seu host): `localhost` e porta `3307`
- User: `root`
- Password: `example`
- Database padr�o criado: `atria`

Acessando Adminer
- Abra http://localhost:8080
- Em "System" selecione `MySQL` (ou deixe em branco)
- Host: `db` (ou `localhost`+porta `3307` se conectar externamente)
- Username: `root`
- Password: `example`
- Database: `atria`

Migra��es
- O projeto aplica migra��es automaticamente na inicializa��o da API (ver `Program.cs`).
- Para aplicar migra��es manualmente (local):
  - No projeto raiz: `dotnet ef database update --project src/3. Infrastructure --startup-project src/4. Api`
  - Criar migra��o: `dotnet ef migrations add NomeDaMigration --project src/3. Infrastructure --startup-project src/4. Api`

Executando localmente sem Docker
- Configure a string de conex�o em `src/4. Api/appsettings.Development.json` ou use vari�vel de ambiente `ConnectionStrings__DefaultConnection`.
- Rode: `dotnet run --project src/4. Api`

Parar e remover containers
- Parar: `docker compose down`
- Parar e remover volumes (dados do DB): `docker compose down -v` (ATEN��O: remove dados do banco)

Dicas de seguran�a
- N�o comitar segredos (senhas, chaves). Use vari�veis de ambiente ou `dotnet user-secrets` em dev.
- Em produ��o, n�o persista as chaves de DataProtection dentro do container sem um reposit�rio persistente.

Suporte
- Se houver erro ao criar a imagem, verifique se o Docker daemon est� ativo e se o contexto de build inclui os projetos referenciados (o `docker-compose.yml` do reposit�rio j� est� configurado para buildar a solu��o).

