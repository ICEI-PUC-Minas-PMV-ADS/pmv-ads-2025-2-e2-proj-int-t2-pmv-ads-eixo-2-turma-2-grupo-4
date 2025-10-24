# Registro de Testes de Software

<span style="color:red">Pré-requisitos: <a href="04-Projeto de Interface.md"> Projeto de Interface</a></span>, <a href="08-Plano de Testes de Software.md"> Plano de Testes de Software</a>

Para cada caso de teste definido no Plano de Testes de Software, realize o registro das evidências dos testes feitos na aplicação pela equipe, que comprovem que o critério de êxito foi alcançado (ou não!!!). Para isso, utilize uma ferramenta de captura de tela que mostre cada um dos casos de teste definidos (obs.: cada caso de teste deverá possuir um vídeo do tipo _screencast_ para caracterizar uma evidência do referido caso).

| **Caso de Teste** 	| **CT01 – Cadastrar Novo Usuário** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-001 - O sistema deve permitir que um novo usuário realize um cadastro pessoal na plataforma. |
|Registro de evidência | ✅ Implementado - AccountController.Register realiza cadastro com validação de campos (Nome, Email, Senha, TipoUsuario), atribui roles automaticamente (Professor/Comum), e redireciona para Home após sucesso. |

| **Caso de Teste** 	| **CT02 – Efetuar Login** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-002 - O sistema deve permitir que um usuário já cadastrado efetue login na plataforma. |
|Registro de evidência | ✅ Implementado - AccountController.Login autentica usuário via SignInManager usando email e senha, suporta "Remember Me", possui proteção contra lockout, e redireciona para URL de origem. |

| **Caso de Teste** 	| **CT03 – Realizar Busca Avançada** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-003 - O sistema deve possuir uma ferramenta de busca avançada que permita ao usuário encontrar materiais por palavra-chave, autor, ano de publicação, categoria e/ou termos técnicos específicos. |
|Registro de evidência | ⚠️
| **Caso de Teste** 	| **CT04 – Avaliar Material** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-004 - O sistema deve permitir que usuários e professores avaliem os materiais. |
|Registro de evidência | ✅ Implementado - AvaliacoesController.Create permite criar avaliações vinculando usuário automaticamente, suporta Nota, TipoAvaliacao (diferencia professor), Resenha opcional, e redireciona para detalhes do material. |

| **Caso de Teste** 	| **CT05 – Criar Comunidade de Discussão** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-005 - O sistema deve permitir a criação de comunidades de discussão temáticos. |
|Registro de evidência | ✅ Implementado - ComunidadesController.Create permite criação de comunidades com Nome e Descrição, registra DataCriacao automaticamente, possui validação ModelState, e redireciona para listagem. |

| **Caso de Teste** 	| **CT06 – Criar Postagem em Comunidade** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-006 - O sistema deve permitir que o usuário crie postagens dentro das comunidades. |
|Registro de evidência | ✅ Implementado - PostagensController.Create vincula postagem ao usuário logado automaticamente, permite associar FKComunidade (opcional), registra DataPostagem, suporta flag NoForumGeral, e valida campos obrigatórios. |

| **Caso de Teste** 	| **CT07 – Comentar em uma Postagem** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-007 - O sistema deve permitir que os usuários comentem nas postagens. |
|Registro de evidência | 

| **Caso de Teste** 	| **CT08 – Moderar Conteúdo** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-008 - O sistema deve possuir ferramentas de moderação para que administradores ou moderadores possam revisar e remover conteúdos inadequados. |
|Registro de evidência | 

| **Caso de Teste** 	| **CT09 – Ordenar por Popularidade** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-009 - O sistema deve permitir que o usuário visualize os materiais com base em rankings de popularidade da comunidade. |
|Registro de evidência |

| **Caso de Teste** 	| **CT10 – Criar Lista de Leitura** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-010 / RF-017 - O sistema deve permitir que o usuário crie e compartilhe listas de leitura personalizadas. |
|Registro de evidência | ✅ Implementado - ListasLeituraController.Create cria listas vinculadas ao usuário (FKUsuario), suporta Nome e Descrição. Modelo ListaTemMaterial permite relacionamento many-to-many com materiais. Falta interface para adicionar/remover materiais da lista. |

| **Caso de Teste** 	| **CT11 – Personalizar Perfil com Área de Estudo** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-011 - O sistema deve permitir que o usuário personalize seu perfil, adicionando a informação de área de estudo. |
|Registro de evidência | ✅ **IMPLEMENTADO E TESTADO** - Campo AreaEstudo adicionado ao modelo ApplicationUser (varchar 200). ProfileController com actions Index (visualização) e Edit (edição) implementadas. Views Profile/Index.cshtml e Profile/Edit.cshtml criadas com formulário funcional. Validação MaxLength(200) ativa. Migration AdicionarCamposPerfilUsuario aplicada ao banco MySQL. Link "Meu Perfil" adicionado ao menu principal com ícone Bootstrap. Teste realizado em localhost:5000 com sucesso - usuário admin@local.test conseguiu editar área de estudo e visualizar dados salvos. |

| **Caso de Teste** 	| **CT12 – Criar Grupo de Estudo** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-012 - O sistema deve permitir a criação de grupos de estudo sobre tópicos específicos. |
|Registro de evidência | ✅ Implementado - GruposEstudoController.Create permite criação de grupos com Nome, Descrição, FKComunidade (opcional), registra DataCriacao automaticamente. Modelo UsuarioGrupo permite relacionamento many-to-many com usuários. |

| **Caso de Teste** 	| **CT13 – Adicionar Novo Material para Revisão** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-013 - O sistema deve permitir que usuários adicionem novos materiais, que passarão por uma revisão e aprovação de moderadores. |
|Registro de evidência | ✅ Implementado - MateriaisController.Create permite adicionar materiais com campos Titulo, Descricao, Tipo, Status. Vincula FKUsuarioCriador automaticamente, registra DataCriacao. Campo Status permite controle de aprovação (ex: "Pendente", "Aprovado"). |

| **Caso de Teste** 	| **CT14 – Receber Notificação** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-014 - O sistema deve enviar notificações aos usuários sobre novas respostas em suas postagens. |
|Registro de evidência |  

| **Caso de Teste** 	| **CT15 – Seguir uma Comunidade** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-015 - O sistema deve permitir que os usuários sigam comunidades específicas ou outros usuários. |
|Registro de evidência ||

| **Caso de Teste** 	| **CT16 – Criar Postagem no Fórum Geral** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-016 - O sistema terá um Fórum geral para as postagens abertas feitas pelos usuários. |
|Registro de evidência | ✅ Implementado - Modelo Postagem possui campo NoForumGeral (bool) e FKComunidade (nullable). PostagensController.Create permite criar postagens sem vínculo a comunidade específica. Campo NoForumGeral controla visibilidade no fórum geral. |

| **Caso de Teste** 	| **CT17 – Ordenar por Melhores Avaliações** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-018 - O sistema deve permitir que o usuário visualize os materiais pelas melhores avaliações da comunidade. |
|Registro de evidência | |

| **Caso de Teste** 	| **CT18 – Personalizar Perfil com Trilha de Conhecimento** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-019 - O sistema deve permitir que o usuário personalize seu perfil, adicionando a informação de trilha de conhecimento. |
|Registro de evidência | ✅ **IMPLEMENTADO E TESTADO** - Campo TrilhaConhecimento adicionado ao modelo ApplicationUser (varchar 500). ProfileController implementa visualização (GET Index) e edição (GET/POST Edit). View Profile/Edit.cshtml possui textarea específica para trilha de conhecimento com validação MaxLength(500) e mensagens informativas. Migration AdicionarCamposPerfilUsuario aplicada com sucesso ao banco de dados MySQL. Link "👤 Meu Perfil" com ícone Bootstrap Icons adicionado ao menu de navegação (_Layout.cshtml). Teste realizado em http://localhost:5000 - usuário admin@local.test acessou /Profile/Edit, preencheu campo "Trilha de Conhecimento" com texto multilinha ("Desenvolvimento Web com ASP.NET Core, Entity Framework Core e MySQL, Design Patterns e Clean Architecture"), salvou com sucesso e visualizou dados em /Profile/Index. TempData exibiu mensagem "Perfil atualizado com sucesso!". Campo aceita múltiplas linhas e exibe corretamente na visualização. |

| **Caso de Teste** 	| **CT19 – Personalizar Perfil com Projetos** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-020 - O sistema deve permitir que o usuário personalize seu perfil, adicionando a informação de projetos aos quais participa. |
|Registro de evidência | ✅ **IMPLEMENTADO E TESTADO** - Campo Projetos adicionado ao modelo ApplicationUser (varchar 1000). ProfileController.Edit permite adicionar e editar projetos. Campo implementado como textarea com suporte a múltiplas linhas (white-space: pre-wrap na view). Validação MaxLength(1000) implementada. Views exibem projetos formatados corretamente preservando quebras de linha. Teste realizado em localhost - usuário conseguiu inserir descrição de projetos em múltiplas linhas e visualizar formatação correta em /Profile/Index. Migration aplicada ao banco com sucesso. |

| **Caso de Teste** 	| **CT20 – Buscar Grupo de Estudo** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-021 - O sistema deve permitir a busca por grupos de estudo sobre tópicos específicos. |


> **Links Úteis**:
> - [Ferramentas de Test para Java Script](https://geekflare.com/javascript-unit-testing/)
> 