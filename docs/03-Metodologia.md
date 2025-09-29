# Metodologia

Este documento descreve a metodologia, as ferramentas e os processos adotados pela equipe para o gerenciamento e desenvolvimento do projeto, garantindo transparência, colaboração e eficiência na entrega da solução.

### Gestão de Times e Processo

A equipe se organiza utilizando um processo ágil, com um quadro Kanban implementado na ferramenta **GitHub Projects** para gerenciar o fluxo de trabalho. O quadro visualiza todas as etapas do desenvolvimento, desde a concepção até a conclusão, e está estruturado com as seguintes colunas:

-   **Backlog:** Contém todas as tarefas, funcionalidades (`features`), melhorias (`enhancements`) e correções (`bugs`) a serem feitas no projeto. É o repositório principal de trabalho, gerenciado pelo Product Owner.
-   **Ready:** Tarefas do Backlog que já foram refinadas, detalhadas e estão prontas para serem iniciadas pela equipe de desenvolvimento.
-   **In Progress:** Tarefas que estão sendo ativamente desenvolvidas por um membro da equipe.
-   **In Review:** A tarefa foi concluída do ponto de vista do desenvolvimento e um *Pull Request* foi aberto. O código está aguardando a revisão de outros membros da equipe para garantir a qualidade e o cumprimento dos requisitos.
-   **Done:** O *Pull Request* foi aprovado e o código foi integrado (merge) à branch correspondente. A tarefa é considerada concluída.

## Controle de Versão

A ferramenta de controle de versão adotada no projeto foi o **Git**, sendo que o **GitHub** foi utilizado para hospedagem do repositório central e para facilitar a colaboração entre os membros da equipe.

### Estratégia de Branches

O projeto segue um fluxo de trabalho baseado em branches com responsabilidades bem definidas, garantindo que a versão em produção (`main`) permaneça sempre estável.

-   `main`: Contém a versão de produção, estável e testada. Nenhum desenvolvimento é feito diretamente aqui.
-   `unstable`: Versão que agrega funcionalidades já testadas, mas que ainda pode conter instabilidades. Serve como um ambiente de pré-lançamento.
-   `testing`: Branch dedicada aos testes de novas funcionalidades. O código da branch `dev` é enviado para cá para validação.
-   `dev`: Principal branch de desenvolvimento. Todas as novas funcionalidades e correções são integradas aqui antes de seguirem para o ambiente de testes.

O fluxo de trabalho geral é: O desenvolvimento acontece na `dev` -> quando um conjunto de funcionalidades está pronto, ele é enviado para a `testing` -> após a aprovação nos testes, o código vai para a `unstable` -> finalmente, quando uma versão está pronta para lançamento, a `unstable` é integrada à `main`.

### Commits, Merges e Tags

-   **Commits:** Devem ser atômicos e possuir mensagens claras e descritivas, seguindo um padrão para facilitar a leitura do histórico do projeto.
-   **Merges:** A integração de código entre as branches (especialmente para `testing`, `unstable` e `main`) é realizada exclusivamente através de **Pull Requests (PRs)** no GitHub. Cada PR deve ser revisado por pelo menos um outro membro da equipe antes de ser aprovado, garantindo a qualidade e a conformidade do código.
-   **Tags:** São utilizadas para marcar marcos importantes, principalmente para criar releases e versionar o software na branch `main` (ex: `v1.0`, `v1.1`, `v2.0`).

### Gerência de Issues

O controle de tarefas, bugs e novas funcionalidades é feito através das **Issues** do GitHub. Elas são a base para a criação dos cartões no quadro Kanban. Para organizar e categorizar as issues, o projeto adota as seguintes etiquetas:

-   `documentation`: Melhorias ou acréscimos à documentação do projeto.
-   `bug`: Relata uma funcionalidade que se encontra com problemas ou comportamento inesperado.
-   `enhancement`: Sugere uma melhoria em uma funcionalidade já existente.
-   `feature`: Descreve uma nova funcionalidade que precisa ser implementada.

# Gerenciamento de Projeto

### Divisão de Papéis

A equipe adota o framework Scrum, que define papéis e responsabilidades claros para otimizar o fluxo de trabalho. A divisão de papéis no grupo foi estabelecida da seguinte forma:

-   **Product Owner (PO):** Isabela Bento
    -   *Responsabilidade:* Representar os interesses dos stakeholders, definir as funcionalidades do produto e gerenciar o Product Backlog, garantindo que a equipe de desenvolvimento entregue o maior valor possível.

-   **Scrum Master:** Thiago Luigi
    -   *Responsabilidade:* Garantir que o time siga os princípios e práticas do Scrum, remover impedimentos que possam atrapalhar a equipe, facilitar as cerimônias e promover um ambiente de melhoria contínua.

-   **Equipe de Desenvolvimento:** Ícaro Oliveira de Freitas, Sendrick Paz Muniz dos Reis, Denys Makene Maia e Silva, Higor Henrique Pereira Zanhe
    -   *Responsabilidade:* Equipe multidisciplinar responsável por desenvolver e entregar o incremento do produto ao final de cada Sprint. São auto-organizáveis e possuem as habilidades necessárias para transformar os itens do Backlog em uma solução funcional.

-   **Designer (UX/UI):** João Carlos Pires Padilha França
    -   *Responsabilidade:* Criar a identidade visual, os fluxos de navegação e as interfaces da solução (wireframes e protótipos), trabalhando em estreita colaboração com o Product Owner e a Equipe de Desenvolvimento para garantir uma experiência de usuário intuitiva e eficaz.


### Processo

O grupo utiliza o **Scrum** como framework ágil para gerenciar o projeto. A implementação se baseia na organização de Sprints (ciclos de trabalho curtos) e no uso de ferramentas online para garantir o acompanhamento e a visibilidade do progresso.

O fluxo de trabalho é gerenciado através da funcionalidade **GitHub Projects**, onde foi configurado um quadro no estilo **Kanban**. Este quadro é a principal ferramenta para a gestão visual das tarefas e possui colunas que representam o fluxo de trabalho, como:

-   **Backlog:** Lista de todas as funcionalidades e tarefas a serem desenvolvidas.
-   **To Do (A Fazer):** Tarefas selecionadas do Backlog para serem executadas na Sprint atual.
-   **In Progress (Em Andamento):** Tarefas que estão sendo desenvolvidas ativamente.
-   **Done (Concluído):** Tarefas finalizadas e que atendem aos critérios de aceitação.

As cerimônias do Scrum (reuniões de planejamento, reuniões diárias, revisão e retrospectiva da Sprint) são realizadas de forma remota através do **Discord**, permitindo um acompanhamento contínuo e a resolução rápida de impedimentos.


### Ferramentas

A seleção de ferramentas foi pautada na integração, colaboração e adequação às necessidades do projeto. A seguir, são listadas as ferramentas empregadas e as justificativas para suas escolhas.

| Categoria                      | Ferramenta(s)                     | Justificativa da Escolha                                                                                                                                                                                                                         |
| :----------------------------- | :-------------------------------- | :----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Editor de Código** | `Visual Studio` e `VS Code`       | Escolhidos pela sua alta performance e, principalmente, pela **integração nativa com o Git e o GitHub**. Isso otimiza o fluxo de trabalho do desenvolvedor, permitindo versionar o código e gerenciar branches diretamente do editor.               |
| **Comunicação** | `Discord` e `GitHub`              | O **Discord** foi selecionado para comunicação síncrona (reuniões e discussões em tempo real). O **GitHub** é utilizado para a comunicação assíncrona, focada em discussões técnicas dentro de *Issues* e *Pull Requests*.                           |
| **Desenho de Tela (Wireframing)** | `Figma`                           | A escolha pelo **Figma** se deu por ser uma ferramenta colaborativa que **melhor capturou as necessidades de design da solução**. Ele permite criar, compartilhar e receber feedbacks sobre wireframes e protótipos em tempo real.                |
| **Gerenciamento e Versionamento** | `GitHub`                          | Além de ser um repositório, o **GitHub** centraliza o gerenciamento do projeto através do **GitHub Projects**. A capacidade de vincular *commits* e *pull requests* às tarefas no quadro Kanban proporciona uma rastreabilidade completa.          |

