# Plano de Testes de Software

| **Caso de Teste** | **CT01 – Cadastrar Novo Usuário** |
|:---:|:---:|
| Requisito Associado | RF-001 - O sistema deve permitir que um novo usuário realize um cadastro pessoal na plataforma. |
| Objetivo do Teste | Verificar se um novo usuário consegue se cadastrar na plataforma. |
| Passos | - Acessar o navegador de sua preferencia <br> - Acessar a página inicial da aplicação <br> - Clicar no botão "Cadastrar-se" ou "Criar Conta" <br> - Preencher os campos obrigatórios (ex: Nome, E-mail, Senha) <br> - Clicar no botão "Finalizar Cadastro" |
| Critério de Êxito | - O cadastro é realizado com sucesso e o usuário é redirecionado para a página de login ou painel principal. |
| | |
| **Caso de Teste** | **CT02 – Efetuar Login** |
| Requisito Associado | RF-002 - O sistema deve permitir que um usuário já cadastrado efetue login na plataforma. |
| Objetivo do Teste | Verificar se um usuário já cadastrado consegue realizar o login. |
| Passos | - Acessar a página de login <br> - Preencher o campo de e-mail com um e-mail cadastrado <br> - Preencher o campo da senha com a senha correta <br> - Clicar em "Entrar" |
| Critério de Êxito | - O login foi realizado com sucesso e o usuário acessa a área logada do sistema. |
| | |
| **Caso de Teste** | **CT03 – Realizar Busca Avançada** |
| Requisito Associado | RF-003 - O sistema deve possuir uma ferramenta de busca avançada que permita ao usuário encontrar materiais por palavra-chave, autor, ano de publicação, categoria e/ou termos técnicos específicos. |
| Objetivo do Teste | Verificar se a busca avançada retorna resultados corretos ao usar múltiplos filtros. |
| Passos | - Acessar a página de busca <br> - Preencher o campo "palavra-chave" <br> - Selecionar um "autor" no filtro <br> - Definir um "ano de publicação" <br> - Clicar em "Buscar" |
| Critério de Êxito | - O sistema exibe apenas os resultados que correspondem a todos os filtros aplicados. |
| | |
| **Caso de Teste** | **CT04 – Avaliar Material** |
| Requisito Associado | RF-004 - O sistema deve permitir que usuários e professores avaliem os materiais. |
| Objetivo do Teste | Verificar se um usuário consegue registrar uma avaliação em um material. |
| Passos | - Efetuar login na plataforma <br> - Acessar a página de um material <br> - Selecionar uma nota (ex: 4 estrelas) <br> - Escrever um comentário de avaliação (opcional) <br> - Clicar em "Enviar Avaliação" |
| Critério de Êxito | - A avaliação do usuário é registrada e exibida na página do material. |
| | |
| **Caso de Teste** | **CT05 – Criar Comunidade de Discussão** |
| Requisito Associado | RF-005 - O sistema deve permitir a criação de comunidades de discussão temáticos. |
| Objetivo do Teste | Verificar se o usuário consegue criar uma nova comunidade. |
| Passos | - Acessar a área de "Comunidades" <br> - Clicar em "Criar Nova Comunidade" <br> - Preencher os campos de nome e descrição da comunidade <br> - Clicar em "Criar" |
| Critério de Êxito | - A comunidade é criada com sucesso e exibida na lista de comunidades. |
| | |
| **Caso de Teste** | **CT06 – Criar Postagem em Comunidade** |
| Requisito Associado | RF-006 - O sistema deve permitir que o usuário crie postagens dentro das comunidades. |
| Objetivo do Teste | Verificar se o usuário consegue criar uma postagem dentro de uma comunidade. |
| Passos | - Acessar uma comunidade <br> - Clicar em "Nova Postagem" <br> - Preencher o título e o corpo da postagem <br> - Clicar em "Publicar" |
| Critério de Êxito | - A postagem é publicada com sucesso no feed da comunidade. |
| | |
| **Caso de Teste** | **CT07 – Comentar em uma Postagem** |
| Requisito Associado | RF-007 - O sistema deve permitir que os usuários comentem nas postagens. |
| Objetivo do Teste | Verificar se o usuário consegue adicionar um comentário a uma postagem existente. |
| Passos | - Acessar uma postagem existente <br> - Digitar um texto no campo de comentário <br> - Clicar no botão "Comentar" ou "Enviar" |
| Critério de Êxito | - O comentário é exibido com sucesso abaixo da postagem. |
| | |
| **Caso de Teste** | **CT08 – Moderar Conteúdo** |
| Requisito Associado | RF-008 - O sistema deve possuir ferramentas de moderação para que administradores ou moderadores possam revisar e remover conteúdos inadequados. |
| Objetivo do Teste | Verificar se um usuário moderador consegue remover uma postagem. |
| Passos | - Efetuar login com um perfil de moderador <br> - Navegar até uma postagem criada por outro usuário <br> - Clicar na opção "Moderar" ou "Remover" <br> - Confirmar a remoção |
| Critério de Êxito | - A postagem foi removida com sucesso e não está mais visível para os usuários. |
| | |
| **Caso de Teste** | **CT09 – Ordenar por Popularidade** |
| Requisito Associado | RF-009 - O sistema deve permitir que o usuário visualize os materiais com base em rankings de popularidade da comunidade. |
| Objetivo do Teste | Verificar se a ordenação de materiais por popularidade funciona. |
| Passos | - Acessar a lista de materiais <br> - Localizar o menu de ordenação <br> - Selecionar a opção "Mais Populares" |
| Critério de Êxito | - A lista de materiais é reordenada, mostrando os mais populares primeiro. |
| | |
| **Caso de Teste** | **CT10 – Criar Lista de Leitura** |
| Requisito Associado | RF-010 / RF-017 - O sistema deve permitir que o usuário crie e compartilhe listas de leitura personalizadas. |
| Objetivo do Teste | Verificar se o usuário consegue criar uma lista de leitura e adicionar um item a ela. |
| Passos | - Acessar a seção "Minhas Listas" <br> - Clicar em "Criar Nova Lista" e dar um nome a ela <br> - Navegar até um material de interesse <br> - Clicar em "Adicionar à Lista" <br> - Selecionar a lista criada |
| Critério de Êxito | - O material é adicionado com sucesso à lista de leitura selecionada. |
| | |
| **Caso de Teste** | **CT11 – Personalizar Perfil com Área de Estudo** |
| Requisito Associado | RF-011 - O sistema deve permitir que o usuário personalize seu perfil, adicionando a informação de área de estudo. |
| Objetivo do Teste | Verificar se o usuário consegue adicionar ou editar sua área de estudo no perfil. |
| Passos | - Acessar a página de "Editar Perfil" <br> - Preencher ou alterar o campo "Área de Estudo" <br> - Clicar em "Salvar Alterações" |
| Critério de Êxito | - A informação da área de estudo foi salva e é exibida no perfil do usuário. |
| | |
| **Caso de Teste** | **CT12 – Criar Grupo de Estudo** |
| Requisito Associado | RF-012 - O sistema deve permitir a criação de grupos de estudo sobre tópicos específicos. |
| Objetivo do Teste | Verificar se o usuário consegue criar um novo grupo de estudo. |
| Passos | - Acessar a área de "Grupos de Estudo" <br> - Clicar em "Criar Novo Grupo" <br> - Preencher as informações do grupo (nome, tópico) <br> - Clicar em "Criar" |
| Critério de Êxito | - O grupo de estudo foi criado com sucesso. |
| | |
| **Caso de Teste** | **CT13 – Adicionar Novo Material para Revisão** |
| Requisito Associado | RF-013 - O sistema deve permitir que usuários adicionem novos materiais, que passarão por uma revisão e aprovação de moderadores. |
| Objetivo do Teste | Verificar se o usuário consegue submeter um novo material para aprovação. |
| Passos | - Acessar a funcionalidade "Adicionar Material" <br> - Preencher todos os campos do formulário do material <br> - Clicar em "Enviar para Revisão" |
| Critério de Êxito | - O material é enviado com sucesso e fica com o status "Pendente de Aprovação". |
| | |
| **Caso de Teste** | **CT14 – Receber Notificação** |
| Requisito Associado | RF-014 - O sistema deve enviar notificações aos usuários sobre novas respostas em suas postagens. |
| Objetivo do Teste | Verificar se o usuário recebe uma notificação quando sua postagem é comentada. |
| Passos | - Com o Usuário A, criar uma postagem <br> - Fazer login com o Usuário B <br> - Comentar na postagem do Usuário A <br> - Fazer login novamente com o Usuário A <br> - Verificar o ícone/área de notificações |
| Critério de Êxito | - Uma notificação sobre o novo comentário é exibida para o Usuário A. |
| | |
| **Caso de Teste** | **CT15 – Seguir uma Comunidade** |
| Requisito Associado | RF-015 - O sistema deve permitir que os usuários sigam comunidades específicas ou outros usuários. |
| Objetivo do Teste | Verificar se o usuário consegue seguir uma comunidade. |
| Passos | - Acessar a página de uma comunidade <br> - Clicar no botão "Seguir" ou "Participar" |
| Critério de Êxito | - O usuário passa a seguir a comunidade e o botão muda para "Seguindo". |
| | |
| **Caso de Teste** | **CT16 – Criar Postagem no Fórum Geral** |
| Requisito Associado | RF-016 - O sistema terá um Fórum geral para as postagens abertas feitas pelos usuários. |
| Objetivo do Teste | Verificar se o usuário consegue criar uma postagem que não pertence a nenhuma comunidade específica. |
| Passos | - Acessar a área do "Fórum Geral" <br> - Clicar em "Criar Nova Postagem" <br> - Preencher o título e o corpo da postagem <br> - Clicar em "Publicar" |
| Critério de Êxito | - A postagem foi criada com sucesso e está visível no Fórum Geral. |
| | |
| **Caso de Teste** | **CT17 – Ordenar por Melhores Avaliações** |
| Requisito Associado | RF-018 - O sistema deve permitir que o usuário visualize os materiais pelas melhores avaliações da comunidade. |
| Objetivo do Teste | Verificar se a ordenação de materiais por avaliação funciona corretamente. |
| Passos | - Acessar a lista de materiais <br> - Localizar o menu de ordenação <br> - Selecionar a opção "Melhores Avaliações" |
| Critério de Êxito | - A lista de materiais é reordenada, mostrando os materiais com maiores notas primeiro. |
| | |
| **Caso de Teste** | **CT18 – Personalizar Perfil com Trilha de Conhecimento** |
| Requisito Associado | RF-019 - O sistema deve permitir que o usuário personalize seu perfil, adicionando a informação de trilha de conhecimento. |
| Objetivo do Teste | Verificar se o usuário consegue adicionar ou editar sua trilha de conhecimento no perfil. |
| Passos | - Acessar a página de "Editar Perfil" <br> - Preencher ou alterar o campo "Trilha de Conhecimento" <br> - Clicar em "Salvar Alterações" |
| Critério de Êxito | - A informação da trilha de conhecimento foi salva e é exibida no perfil do usuário. |
| | |
| **Caso de Teste** | **CT19 – Personalizar Perfil com Projetos** |
| Requisito Associado | RF-020 - O sistema deve permitir que o usuário personalize seu perfil, adicionando a informação de projetos aos quais participa. |
| Objetivo do Teste | Verificar se o usuário consegue adicionar ou editar seus projetos no perfil. |
| Passos | - Acessar a página de "Editar Perfil" <br> - Preencher ou alterar o campo "Projetos" <br> - Clicar em "Salvar Alterações" |
| Critério de Êxito | - A informação de projetos foi salva e é exibida no perfil do usuário. |
| | |
| **Caso de Teste** | **CT20 – Buscar Grupo de Estudo** |
| Requisito Associado | RF-021 - O sistema deve permitir a busca por grupos de estudo sobre tópicos específicos. |
| Objetivo do Teste | Verificar se a busca por grupos de estudo retorna resultados relevantes. |
| Passos | - Acessar a página de "Grupos de Estudo" <br> - Utilizar a barra de busca para pesquisar por um tópico (ex: "Direito") <br> - Clicar em "Buscar" |
| Critério de Êxito | - A busca retorna uma lista de grupos de estudo relacionados ao tópico pesquisado. |

