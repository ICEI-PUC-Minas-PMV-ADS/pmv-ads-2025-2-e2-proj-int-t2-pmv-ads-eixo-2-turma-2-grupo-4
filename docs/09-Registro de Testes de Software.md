# Registro de Testes de Software

<span style="color:red">Pré-requisitos: <a href="04-Projeto de Interface.md"> Projeto de Interface</a></span>, <a href="08-Plano de Testes de Software.md"> Plano de Testes de Software</a>

Para cada caso de teste definido no Plano de Testes de Software, realize o registro das evidências dos testes feitos na aplicação pela equipe, que comprovem que o critério de êxito foi alcançado (ou não!!!). Para isso, utilize uma ferramenta de captura de tela que mostre cada um dos casos de teste definidos (obs.: cada caso de teste deverá possuir um vídeo do tipo _screencast_ para caracterizar uma evidência do referido caso).

| **Caso de Teste** 	| **CT01 – Cadastrar Novo Usuário** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-001 - O sistema deve permitir que um novo usuário realize um cadastro pessoal na plataforma. |

https://github.com/user-attachments/assets/cc295382-8750-4e4b-8182-61ca423b4043

| **Titulo** 	| **CT18 – Personalizar Perfil com Trilha de Conhecimento** 	|
|:---:	|:---:	|
|Registro de evidência | AccountController.Register realiza cadastro com validação de campos (Nome, Email, Senha, TipoUsuario), atribui roles automaticamente (Professor/Comum), e redireciona para Home após sucesso. |

| **Caso de Teste** 	| **CT02 – Efetuar Login** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-002 - O sistema deve permitir que um usuário já cadastrado efetue login na plataforma. |

https://github.com/user-attachments/assets/45b32ae2-3dbe-494f-ab15-f01df3c00197

| **Titulo** 	| **CT18 – Personalizar Perfil com Trilha de Conhecimento** 	|
|:---:	|:---:	|
|Registro de evidência | AccountController.Login autentica usuário via SignInManager usando email e senha, suporta "Remember Me", possui proteção contra lockout, e redireciona para URL de origem. |

| **Caso de Teste** 	| **CT04 – Avaliar Material** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-004 - O sistema deve permitir que usuários e professores avaliem os materiais. |

https://github.com/user-attachments/assets/b91fc197-9419-4077-b4c7-e62cbd14c7ce

| **Titulo** 	| **CT18 – Personalizar Perfil com Trilha de Conhecimento** 	|
|:---:	|:---:	|
|Registro de evidência | AvaliacoesController.Create permite criar avaliações vinculando usuário automaticamente, suporta Nota, TipoAvaliacao (diferencia professor), Resenha opcional, e redireciona para detalhes do material. |

| **Caso de Teste** 	| **CT05 – Criar Comunidade de Discussão** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-005 - O sistema deve permitir a criação de comunidades de discussão temáticos. |

https://github.com/user-attachments/assets/17fecdf3-a82b-4993-945b-b0c2cb06aa88

| **Titulo** 	| **CT18 – Personalizar Perfil com Trilha de Conhecimento** 	|
|:---:	|:---:	|
|Registro de evidência | ComunidadesController.Create permite criação de comunidades com Nome e Descrição, registra DataCriacao automaticamente, possui validação ModelState, e redireciona para listagem. |

| **Caso de Teste** 	| **CT06 – Criar Postagem em Comunidade** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-006 - O sistema deve permitir que o usuário crie postagens dentro das comunidades. |

https://github.com/user-attachments/assets/60507792-d699-48db-8d4d-9d771615d5ab

| **Titulo** 	| **CT18 – Personalizar Perfil com Trilha de Conhecimento** 	|
|:---:	|:---:	|
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

https://github.com/user-attachments/assets/4651aacc-d095-41c0-85c0-f727c627cffd

| **Titulo** 	| **CT18 – Personalizar Perfil com Trilha de Conhecimento** 	|
|:---:	|:---:	|
|Registro de evidência | Campo AreaEstudo adicionado ao modelo ApplicationUser (varchar 200). ProfileController com actions Index (visualização) e Edit (edição) implementadas. Views Profile/Index.cshtml e Profile/Edit.cshtml criadas com formulário funcional. Validação MaxLength(200) ativa. Migration AdicionarCamposPerfilUsuario aplicada ao banco MySQL. Link "Meu Perfil" adicionado ao menu principal com ícone Bootstrap. |

| **Caso de Teste** 	| **CT12 – Criar Grupo de Estudo** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-012 - O sistema deve permitir a criação de grupos de estudo sobre tópicos específicos. |

https://github.com/user-attachments/assets/0d18b63f-bcb9-4fc4-b94e-2399d6c1350d

| **Titulo** 	| **CT18 – Personalizar Perfil com Trilha de Conhecimento** 	|
|:---:	|:---:	|
|Registro de evidência | GruposEstudoController.Create permite criação de grupos com Nome, Descrição, FKComunidade (opcional), registra DataCriacao automaticamente. Modelo UsuarioGrupo permite relacionamento many-to-many com usuários através de tabela de junção. Controller possui CRUD completo com Index, Details, Edit e Delete. |

| **Caso de Teste** 	| **CT13 – Adicionar Novo Material para Revisão** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-013 - O sistema deve permitir que usuários adicionem novos materiais, que passarão por uma revisão e aprovação de moderadores. |

https://github.com/user-attachments/assets/2972eea1-8ad9-4130-98ae-857edc78fe6e

| **Titulo** 	| **CT18 – Personalizar Perfil com Trilha de Conhecimento** 	|
|:---:	|:---:	|
|Registro de evidência | MateriaisController.Create permite adicionar materiais com campos Titulo, Descricao, Tipo, Status. Vincula FKUsuarioCriador automaticamente ao usuário logado, registra DataCriacao. Campo Status permite controle de aprovação com valores como "Pendente", "Aprovado" ou "Rejeitado". Modelo Material possui relacionamento com ApplicationUser através de FKUsuarioCriador. |

| **Caso de Teste** 	| **CT15 – Seguir uma Comunidade** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-015 - O sistema deve permitir que os usuários sigam comunidades específicas ou outros usuários. |
|Registro de evidência | Modelo UsuarioComunidade existe com relacionamento many-to-many entre Usuario e Comunidade, incluindo campo DataEntrada para rastrear quando usuário entrou na comunidade. Modelo Comunidade possui coleção Usuarios através de UsuarioComunidade. |

| **Caso de Teste** 	| **CT16 – Criar Postagem no Fórum Geral** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-016 - O sistema terá um Fórum geral para as postagens abertas feitas pelos usuários. |

https://github.com/user-attachments/assets/e159e008-35c6-442d-a02d-d84eba691ec2

| **Titulo** 	| **CT18 – Personalizar Perfil com Trilha de Conhecimento** 	|
|:---:	|:---:	|
|Registro de evidência | Modelo Postagem possui campo NoForumGeral (bool) com valor padrão true e FKComunidade (nullable int). PostagensController.Create permite criar postagens sem vínculo a comunidade específica deixando FKComunidade null. Campo NoForumGeral controla visibilidade no fórum geral. Postagens sem comunidade aparecem no fórum geral público. |

| **Caso de Teste** 	| **CT18 – Personalizar Perfil com Trilha de Conhecimento** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-019 - O sistema deve permitir que o usuário personalize seu perfil, adicionando a informação de trilha de conhecimento. |

https://github.com/user-attachments/assets/b4a72264-4310-4295-a927-163787a6ee42

| **Titulo** 	| **CT18 – Personalizar Perfil com Trilha de Conhecimento** 	|
|:---:	|:---:	|
|Registro de evidência | Campo TrilhaConhecimento adicionado ao modelo ApplicationUser (varchar 500). ProfileController implementa visualização (GET Index) e edição (GET/POST Edit). View Profile/Edit.cshtml possui textarea específica para trilha de conhecimento com validação MaxLength(500) e mensagens informativas. Migration AdicionarCamposPerfilUsuario aplicada com sucesso ao banco de dados MySQL. Link "Meu Perfil" adicionado ao menu de navegação (_Layout.cshtml). |

| **Caso de Teste** 	| **CT19 – Personalizar Perfil com Projetos** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-020 - O sistema deve permitir que o usuário personalize seu perfil, adicionando a informação de projetos aos quais participa. |

https://github.com/user-attachments/assets/49e56d55-c6ed-4828-b6c3-478928d11e77

| **Titulo** 	| **CT18 – Personalizar Perfil com Trilha de Conhecimento** 	|
|:---:	|:---:	|
|Registro de evidência | Campo Projetos adicionado ao modelo ApplicationUser (varchar 1000). ProfileController.Edit permite adicionar e editar projetos com GET/POST actions. Campo


