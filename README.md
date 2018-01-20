# O Jogo

  O Jogo foi desenvolvido em C# com propósito único de estudo e prática dos processos de Engenharia de Software.
O projeto utiliza o jogo Tetris como cenário justamente por ser comum para maioria das pessoas, facilitando a correlação das regras no mundo real com os códigos que aqui são escritos.

**Simples**

  Também é propósito desse projeto ser simples. Usar apenas o que agrega valor para a solução e que seja justificadamente necessário. Naturalmente podem haver pontos que se desviam desse propósito, mas assim que percebidos serão refatorados. 

Faço aqui uma referência aos princípios **KISS** e **YAGNI**.

Não tenho objetivo de criar uma estrutura pomposa, que abusa de DLLs e Namespaces para simplesmente fazer a Solution parecer mais complexa, luxuosa, mais elegante ou mais importante do que realmente é. Se fisesse, certamente depois de um tempo isso se tornaria ruído nos projetos, desviaria a atenção do problema ou tornaria a manutenção difícil, cansativa ou desmotivante até o ponto onde esse projeto desse projeto se tornar obsoleto ou inviável.
Mas é importante não confundir isso com **super classes** que abraçam mais responsabilidade do que comportam.

Note que a Solution inicial é composta por dois projetos, o Core e o Desktop.
Quando se modela um Domínio, a neutralidade tecnológica é fundamental, por isso, um Domínio não deve ter ligações externas com Infraestrutura ou Interface de Usuário por exemplo. Assim, alterações de tecnologia não influenciam nas regras de negócio.
No entanto, isolar o Domain em uma DLL à parte não é a única forma de se fazer.
Neste projeto, opto por isolar a Interface e manter uma tríade (Domain, Infra e Services) em uma DLL separada, afinal, nesse projeto em específico o Domínio não é grande o suficiente e a probabilidade de mudanças na Infraestrutura é muito baixa. Com isso, o DomainModel, Infrastructure, Services e CrossCutting ficarão na mesma DLL, que chamarei de Core. Mas perceba que, não há nenhuma classe do Namespace DomainModel fazendo ```using``` de Namespaces como Infrastructure ou Services. Nosso Domínio está isolado e todo o isolamento acontecem normalmente através de modificadores de acesso ```private``` e ```internal```.

**Domain Driven Design**

Normalmente quando se fala em DDD, já vem a cabeça que é só criar uma Class Library pro Domain, uma pra Infra, uma pra Application e um projeto de GUI pra Presentation e o DDD está feito.
Na verdade, tentarei ir um pouco além disso. Gostaria de dar mais expressividade para nossa estrutura e explorar melhor alguns conceitos de DDD.

Na fase de arquitetura propriamente dita é fundamental compreender e respeitar o que a solução em palta realmente necessita. Ter um "Template de DDD", uma estrutura pré-definida é simplesmente uma receita de bolo, uma bala de prata. O importante aqui é compreender a metodologia, os benefícios de cada prática, princípio ou padrão para ter libertade de arquiteturar uma solução.

Para esse projeto tentarei utilizar práticas de DDD que lhe são cabíveis, mas não todas.
Tentei aplicar aqui algumas práticas apesar de não termos complexidade o suficiente para vermos níveis mais avançados de arquitetura, mas teremos uma boa iniciação nas suas práticas básicas, **Modelagem do Domínio** e **Linguagem Ubíqua**.

Neste primeiro projeto o BoundedContext é o próprio jogo Tetris, no futuro outros contextos podem ser adicionados para outros jogos ou modalidades.

Você também não verá nesse projeto pastas chamadas de Entidades, ValueObjects, Interfaces, etc. Veja, esse tipo de modelagem está modelando guiado por *tipo de objeto* e não pelo Domínio.
Os termos Entidades, Interfaces e ValueObjects não são coisas que existem para seu cliente, não fazem parte do domínio dele (a não ser que seu cliente seja uma empresa de software), esses termos ajudam o arquiteto de soluções a identificar um tipo de objeto, mas não precisam ser destacados na arquitetura. Neutralidade tecnológica!

O Domínio precisa ser expressivo, você precisa aprender com ele, falar a lingua dele e utilizar dele para se manter focado no problema (estar no domínio).
Exclusivamente no DomainModel, tento manter ao máximo a linguagem ubíqua, não apenas nomenando os objetos em linguagem natural mas também falando em voz alta e refletindo se o que eu falei faz sentido ou se parecia estar falando com uma máquina!
Eventualmente, farei uso de uma licença poética para mesclar português com inglês, principalmente quando estiver lidando com Patterns.

**Código Limpo**

Várias práticas do Clean Code apontadas por Robert Martin em seu livro, são aplicadas nesse projeto, bons nomes para os objetos e métodos pequenos nem precisam ser citados aqui, mas uma que não é necessariamente do Uncle Bob mas que foi citada por ele, é especial aqui **A Lei de Deméter**.
Isso garantirá que o objeto só acesse propriedades e métodos dele mesmo ou de objetos criados por ele. Evitamos assim aumentar o acoplamento forte de um terceiro objeto.

**Design Pattern**

Tentei aplicar alguns para exemplificá-los na prática, mas ainda deve ter partes a serem refatoradas.
Na primeira publicação veremos pelo menos o uso de State, Strategy, Factory Method, Builder, Bridge, Repository e Controller.

**SOLID**

Assim como no caso dos Design Patterns, é bem possível que achemos partes que violem os princípios SOLID, 
mas mais facilmente veremos boas aplicações de SRP, ISP, DIP (que na minha percepção, pode ser feito sim sem injetores de dependência, por isso é um princípio e não um framework).
Talvez o princípio OCP esteja sendo violado com maior frequência, descubramos!

**Testes**

Aqui não apliquei TDD! Esse péssimo exemplo nos faz chegar ao projeto funcionando mas sem nenhum teste automatizado.
Mas espero escrevê-los em breve, preferenciamente antes de começar a refatorar.

PS: Inclusive estou usando o MSTest rodando com AxoCover e essa tem sido uma boa experiência.
