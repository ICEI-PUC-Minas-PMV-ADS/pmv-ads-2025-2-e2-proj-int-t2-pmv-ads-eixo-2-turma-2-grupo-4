## Instruções de Instalação, Execução e Acesso

Resumo rápido:
- Projeto backend em ASP.NET Core 7 (`src/Atria`).
- Banco de dados: MySQL (Pomelo.EntityFrameworkCore.MySql).
- Identidade: ASP.NET Identity com seed de roles e usuário administrador.

Requisitos:
- .NET 7 SDK (https://dotnet.microsoft.com/)
- MySQL Server (ou MariaDB) acessível
- (Opcional) Visual Studio 2022/2023 ou VS Code
- (Opcional) dotnet-ef: `dotnet tool install --global dotnet-ef`

Configuração:
1. Abra `src/Atria/appsettings.json` e ajuste a connection string `DefaultConnection` para apontar para seu servidor MySQL. Exemplo:

```json
"DefaultConnection": "Server=localhost;Database=atria_db;User=root;Port=3306;Password=123456;"
```

2. Certifique-se de que o usuário e a senha do MySQL possuem permissão para criar e alterar o banco.

Migrações / Banco de Dados:
- Se o projeto já contém migrações, execute (a partir da raiz do repositório):
  - `cd src/Atria`
  - `dotnet ef database update`

- Se não houver migrações, você pode criá-las e aplicá-las:
  - `cd src/Atria`
  - `dotnet ef migrations add InitialCreate`
  - `dotnet ef database update`

Seed e usuário administrador:
- No `Program.cs` há um seeder que cria as roles `Admin`, `Professor` e `Comum` e um usuário administrador padrão caso não exista.
- Credenciais padrão criadas pelo seeder:
  - Email/Username: `admin@local.test`
  - Senha: `Admin@123`
- Observação: o código usa `UserName == Email` para o login; mantenha isso em mente ao alterar usuários.

Executando a aplicação localmente:
1. A partir da raiz do repositório:
   - `cd src/Atria`
   - `dotnet run`
2. O ASP.NET Core irá expor URLs no console (geralmente algo como `https://localhost:5001` / `http://localhost:5000`). Acesse a aplicação e faça login com o usuário administrador acima.

Publicação / Deploy:
- Para publicar uma build de produção:
  - `cd src/Atria`
  - `dotnet publish -c Release -o ./publish`
- Em seguida hospede os arquivos gerados no servidor/serviço de sua escolha (IIS, Docker, Azure App Service, etc.).

Outras informações úteis:
- Código-fonte principal: `src/Atria`
- Contexto do banco: `src/Atria/Data/ApplicationDbContext.cs`
- Modelos: `src/Atria/Models`
- Controllers: `src/Atria/Controllers`
- Views: `src/Atria/Views`
- Documentação do projeto: pasta `docs/`

Acesso rápido (produção) - exemplo (atualize conforme ambiente):
- URL da aplicação: `https://seu-dominio.com/caminho`
- Status: online / instável / em manutenção
- Ambiente: produção / homologação

Usuário(s) de teste (se houver):
- Administrador (seed): `admin@local.test` / `Admin@123`
