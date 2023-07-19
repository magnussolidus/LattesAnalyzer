# LattesAnalyzer

### Este software foi desenvolvido com a finalidade de gerar visualizações para que análises possam ser feitas a respeito dos dados de entrada de maneira mais intuitiva.

1. Este é um programa que recebe um diretório como entrada e produz uma rede como saída.

2. No diretório devem haver os arquivos xml gerado pela plataforma Lattes. 
  - Cada arquivo corresponde a um currículo diferente.
3. São necessário pelo menos dois arquivos para se gerar uma rede. __(recomenda-se o uso de pelo menos 5 arquivos)__

O objetivo deste sistema é facilitar a visualização de redes sociais através das interações que os participantes do [currículo lattes](https://lattes.cnpq.br/) (CV Lattes) tem entre si. 

Ao receber um diretório contendo `N` CV Lattes, onde `N > 1`, o sistema consegue montar um grafo representando as interações entre os participantes com a seguinte representação:
- cada nó representa um pesquisador (ou seja, um cv lattes)
- cada aresta representa que há, pelo menos, uma interação utilizando o critério adotado

É possível adicionar informações extras em arquivos  [GraphML](http://graphml.graphdrawing.org/), devido à sua possibilidade de customização.
No momento, há a implementação de um indicador de índice de centralidade normalizada.

# Requisitos

Este software, em sua versão atual, utiliza [.NET Frameworks 4.5](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net45).
Para salvar o resultado das análises em um arquivo de formato [GraphML](http://graphml.graphdrawing.org/) é necessário executar o software com permissões de administrador.

# Licença

Este projeto utiliza a licença [GNU GPL v3.0](https://www.gnu.org/licenses/gpl-3.0.en.html). Uma cópia do texto da licença, em inglês, está disponível [aqui](Licença/license.md).

# Versões

Este repositório sempre disponibilizará a versão mais recente e estável do sistema.

Há uma versão em C++ com Qt em desenvolvimento, contudo está encontra-se **INSTÁVEL**, por isso não é disponibilizada para o público.
Esta versão está em um repositório particular no [GitLab](https://gitlab.com/).
Caso você tenha interesse em contribuir com o projeto, entre em contato com os desenvolvedores ou nos [envie um e-mail](mailto:magnolomardo@id.uff.br).

# Binários

Caso você não queira gerar os próprios binários, você pode efeturar o donwload de um arquivo compactado (`.rar`) na listagem a seguir: 

- v0.1 - (estável) - [Download aqui](Releases/v0.1/Lattes%20Analyzer%20Release%20v0.0.1.rar)

# Datasets de Exemplo

Como dataset de exemplo, são fornecidos os dados do CV Lattes dos Docentes do [Instituto de Computação](https://www.ic.uff.br/) da [Universidade Federal Fluminense](http://www.uff.br/) na seção [Datasets de Exemplo](Datasets%20de%20Exemplo).

# Melhorias Futuras

Aqui há uma lista de recursos e funcionalidades que incrementariam a funcionalidade do sistema:

- [ ] Melhorar a interface de usuário;
- [ ] Adicionar novos critérios para gerar a rede social a ser visualizada;
- [ ] Adicionar novos indicadores;
- [ ] Adicionar um filtro de seleção de intervalo de tempo;
- [ ] Oferecer suporte à multiplataforma;
- [ ] Consolidar o repositório de desenvolvimento;
- [x] Atualizar a licença para uma licença mais permissiva;

# Histórico de Desenvolvimento

- Inicialmente, desenvolvido para a Disciplina "Inteligência Coletiva", ministrada pelo [Prof. Dr. José Viterbo Filho](http://lattes.cnpq.br/8721187139726277) na [Universidade Federal Fluminense](http://www.uff.br/), 1º período letivo de 2018.
- Versão em C++ com framework Qt 5.x em desenvolvimento em um repositório privado no [GitLab](https://gitlab.com/) desde 2020.
- Versão em C++ com framework Qt 6.x em desenvolvimento em um repositório privado no [GitLab](https://gitlab.com/) desde 2023.
- Posteriormente utilizado em meu Trabalho de Conclusão de Curso de Bacharelado em Sistemas de Informação na [Universidade Federal Fluminense](http://www.uff.br/), em 2023.
