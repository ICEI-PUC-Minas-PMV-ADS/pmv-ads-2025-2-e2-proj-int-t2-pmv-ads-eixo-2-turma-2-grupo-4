# Registro de Testes de Software

<span style="color:red">Pré-requisitos: <a href="04-Projeto de Interface.md"> Projeto de Interface</a></span>, <a href="08-Plano de Testes de Software.md"> Plano de Testes de Software</a>

Para cada caso de teste definido no Plano de Testes de Software, realize o registro das evidências dos testes feitos na aplicação pela equipe, que comprovem que o critério de êxito foi alcançado (ou não!!!). Para isso, utilize uma ferramenta de captura de tela que mostre cada um dos casos de teste definidos (obs.: cada caso de teste deverá possuir um vídeo do tipo _screencast_ para caracterizar uma evidência do referido caso).

| **Caso de Teste** 	| **CT01 – Cadastrar Novo Usuário** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-001 - O sistema deve permitir que um novo usuário realize um cadastro pessoal na plataforma. |
|Registro de evidência | AccountController.Register realiza cadastro com validação de campos (Nome, Email, Senha, TipoUsuario), atribui roles automaticamente (Professor/Comum), e redireciona para Home após sucesso. |

| **Caso de Teste** 	| **CT02 – Efetuar Login** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-002 - O sistema deve permitir que um usuário já cadastrado efetue login na plataforma. |
|Registro de evidência | AccountController.Login autentica usuário via SignInManager usando email e senha, suporta "Remember Me", possui proteção contra lockout, e redireciona para URL de origem. |

| **Caso de Teste** 	| **CT04 – Avaliar Material** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-004 - O sistema deve permitir que usuários e professores avaliem os materiais. |
|Registro de evidência | AvaliacoesController.Create permite criar avaliações vinculando usuário automaticamente, suporta Nota, TipoAvaliacao (diferencia professor), Resenha opcional, e redireciona para detalhes do material. |

| **Caso de Teste** 	| **CT05 – Criar Comunidade de Discussão** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-005 - O sistema deve permitir a criação de comunidades de discussão temáticos. |
|Registro de evidência | ComunidadesController.Create permite criação de comunidades com Nome e Descrição, registra DataCriacao automaticamente, possui validação ModelState, e redireciona para listagem. |

| **Caso de Teste** 	| **CT06 – Criar Postagem em Comunidade** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-006 - O sistema deve permitir que o usuário crie postagens dentro das comunidades. |
|Registro de evidência | PostagensController.Create vincula postagem ao usuário logado automaticamente, permite associar FKComunidade (opcional), registra DataPostagem, suporta flag NoForumGeral, e valida campos obrigatórios. |

| **Caso de Teste** 	| **CT08 – Moderar Conteúdo** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-008 - O sistema deve possuir ferramentas de moderação para que administradores ou moderadores possam revisar e remover conteúdos inadequados. |
|Registro de evidência | Controllers possuem actions Delete (PostagensController.DeleteConfirmed, ComunidadesController.DeleteConfirmed, MateriaisController.DeleteConfirmed) que permitem remoção de conteúdo. Porém, não há controle de permissões específicas para moderadores/administradores - qualquer usuário autenticado pode acessar. |

| **Caso de Teste** 	| **CT10 – Criar Lista de Leitura** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-010 / RF-017 - O sistema deve permitir que o usuário crie e compartilhe listas de leitura personalizadas. |
|Registro de evidência | ListasLeituraController.Create cria listas vinculadas ao usuário (FKUsuario), suporta Nome e Descrição. Modelo ListaTemMaterial permite relacionamento many-to-many com materiais através de tabela de junção com campo Ordem. |

| **Caso de Teste** 	| **CT11 – Personalizar Perfil com Área de Estudo** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-011 - O sistema deve permitir que o usuário personalize seu perfil, adicionando a informação de área de estudo. |
|Registro de evidência | Campo AreaEstudo adicionado ao modelo ApplicationUser (varchar 200). ProfileController com actions Index (visualização) e Edit (edição) implementadas. Views Profile/Index.cshtml e Profile/Edit.cshtml criadas com formulário funcional. Validação MaxLength(200) ativa. Migration AdicionarCamposPerfilUsuario aplicada ao banco MySQL. Link "Meu Perfil" adicionado ao menu principal com ícone Bootstrap. |

| **Caso de Teste** 	| **CT12 – Criar Grupo de Estudo** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-012 - O sistema deve permitir a criação de grupos de estudo sobre tópicos específicos. |
|Registro de evidência | GruposEstudoController.Create permite criação de grupos com Nome, Descrição, FKComunidade (opcional), registra DataCriacao automaticamente. Modelo UsuarioGrupo permite relacionamento many-to-many com usuários através de tabela de junção. Controller possui CRUD completo com Index, Details, Edit e Delete. |

| **Caso de Teste** 	| **CT13 – Adicionar Novo Material para Revisão** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-013 - O sistema deve permitir que usuários adicionem novos materiais, que passarão por uma revisão e aprovação de moderadores. |
|Registro de evidência | MateriaisController.Create permite adicionar materiais com campos Titulo, Descricao, Tipo, Status. Vincula FKUsuarioCriador automaticamente ao usuário logado, registra DataCriacao. Campo Status permite controle de aprovação com valores como "Pendente", "Aprovado" ou "Rejeitado". Modelo Material possui relacionamento com ApplicationUser através de FKUsuarioCriador. |

| **Caso de Teste** 	| **CT15 – Seguir uma Comunidade** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-015 - O sistema deve permitir que os usuários sigam comunidades específicas ou outros usuários. |
|Registro de evidência | Modelo UsuarioComunidade existe com relacionamento many-to-many entre Usuario e Comunidade, incluindo campo DataEntrada para rastrear quando usuário entrou na comunidade. Modelo Comunidade possui coleção Usuarios através de UsuarioComunidade. |

| **Caso de Teste** 	| **CT16 – Criar Postagem no Fórum Geral** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-016 - O sistema terá um Fórum geral para as postagens abertas feitas pelos usuários. |
|Registro de evidência | Modelo Postagem possui campo NoForumGeral (bool) com valor padrão true e FKComunidade (nullable int). PostagensController.Create permite criar postagens sem vínculo a comunidade específica deixando FKComunidade null. Campo NoForumGeral controla visibilidade no fórum geral. Postagens sem comunidade aparecem no fórum geral público. |

| **Caso de Teste** 	| **CT18 – Personalizar Perfil com Trilha de Conhecimento** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-019 - O sistema deve permitir que o usuário personalize seu perfil, adicionando a informação de trilha de conhecimento. |
|Registro de evidência | Campo TrilhaConhecimento adicionado ao modelo ApplicationUser (varchar 500). ProfileController implementa visualização (GET Index) e edição (GET/POST Edit). View Profile/Edit.cshtml possui textarea específica para trilha de conhecimento com validação MaxLength(500) e mensagens informativas. Migration AdicionarCamposPerfilUsuario aplicada com sucesso ao banco de dados MySQL. Link "Meu Perfil" adicionado ao menu de navegação (_Layout.cshtml). |

| **Caso de Teste** 	| **CT19 – Personalizar Perfil com Projetos** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-020 - O sistema deve permitir que o usuário personalize seu perfil, adicionando a informação de projetos aos quais participa. |
|Registro de evidência | Campo Projetos adicionado ao modelo ApplicationUser (varchar 1000). ProfileController.Edit permite adicionar e editar projetos com GET/POST actions. Campo implementado como textarea com suporte a múltiplas linhas (white-space: pre-wrap na view). Validação MaxLength(1000) implementada no modelo e view. Views exibem projetos formatados corretamente preservando quebras de linha. Migration aplicada ao banco com sucesso. |



