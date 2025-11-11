Guia rápido para front-end — Entidade `Postagem`

1) Situação atual
- A classe `Postagem` suporta postagens no fórum geral (`NoForumGeral = true`), em uma comunidade (`FKComunidade`) e em um grupo de estudo (`FKGrupo`).
- O `FKUsuario` é atribuído no servidor a partir do usuário autenticado; o front-end não deve enviar esse campo.
- O `PostagensController` já inclui nas consultas os relacionamentos `Usuario`, `Comunidade` e `GrupoEstudo` para que todas as informações necessárias sejam retornadas.

2) Endpoints / Actions disponíveis (controller `PostagensController`)
- `GET /Postagens` -> action `Index()`
  - Retorna lista de postagens (cada item inclui `Usuario`, `Comunidade`, `GrupoEstudo`).
  - Observação: não há filtro por rota; o front-end pode filtrar a lista por `FKComunidade` ou `FKGrupo` conforme a tela.

- `GET /Postagens/Details/{id}` -> action `Details(id)`
  - Retorna uma postagem específica com relacionamentos.

- `GET /Postagens/Create?comunidadeId={id}&grupoId={id}` -> action `Create(comunidadeId, grupoId)`
  - Usado para abrir a tela de criação já vinculado a uma comunidade ou grupo (parâmetros opcionais).

- `POST /Postagens/Create` -> action `Create(Postagem)`
  - Campos esperados (corpo do form/JSON):
    - `Conteudo` (string, obrigatório)
    - `FKComunidade` (int?, opcional)
    - `FKGrupo` (int?, opcional)
    - `NoForumGeral` (bool)
  - O servidor define `FKUsuario` e `DataPostagem`.

- `GET /Postagens/Edit/{id}` e `POST /Postagens/Edit/{id}` -> edição de postagem (envie `Id, Conteudo, FKComunidade, FKGrupo, NoForumGeral` no POST).
- `GET /Postagens/Delete/{id}` e `POST /Postagens/Delete/{id}` -> exclusão.

3) Estrutura JSON de exemplo retornada por `Index` / `Details`
{
  "id": 123,
  "conteudo": "Texto da postagem",
  "dataPostagem": "2025-11-04T12:34:56Z",
  "noForumGeral": false,
  "fkUsuario": 5,
  "usuario": { "id": 5, "nome": "Usuário Example", "email": "u@ex.com" },
  "fkComunidade": 10, // ou null
  "comunidade": { "id": 10, "nome": "Comunidade X" },
  "fkGrupo": 3, // ou null
  "grupoEstudo": { "id": 3, "nome": "Grupo Y" }
}

4) O que puxar para cada tela
- Tela principal (feed): chamar `GET /Postagens` e mostrar postagens onde `NoForumGeral == true` OU mostrar todas e combinar com o feed desejado. Mostrar `usuario.nome`, `conteudo`, `dataPostagem`, e se houver `comunidade.nome` ou `grupoEstudo.nome` mostrá-los.
- Tela de Comunidade: filtrar a lista por `FKComunidade == {comunidadeId}`; exibir e permitir criação passando `comunidadeId` para `GET /Postagens/Create` ou preenchendo `FKComunidade` no POST.
- Tela de Grupo: filtrar por `FKGrupo == {grupoId}`; criação similar, passando `grupoId`.

5) Observações para o front-end
- Não envie `FKUsuario` nem `DataPostagem` no POST; o servidor atribui automaticamente.
- Campos opcionais: `FKComunidade` e `FKGrupo`. Caso a postagem fique no fórum geral, `NoForumGeral` pode ser `true` e ambos os FK podem ser null.
- Se precisarem de endpoints com filtro no servidor (por comunidade/grupo), posso estender o controller para aceitar query parameters e retornar apenas as postagens desejadas.

Arquivo de referência: `src/Atria/Models/Postagem.cs` e `src/Atria/Controllers/PostagensController.cs`.