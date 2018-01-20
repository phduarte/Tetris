# O Jogo

  O Jogo foi desenvolvido em C# com propósito único de estudo e prática dos processos de Engenharia de Software.
O projeto utiliza o jogo Tetris como cenário justamente por ser comum para maioria das pessoas, facilitanto a correlação das regras no mundo real com os códigos que aqui são escritos.

**Simples**

  Também é propósito desse projeto ser simples. Usar apenas o que agrega valor para a solução e que seja justificadamente necessário. Naturalmente podem haver pontos que se desviam desse propósito, mas assim que percebidos serão refatorados. 

Faço aqui uma referência aos princípios **KISS** e **YAGNI**.

Não tenho objetivo de criar uma estruturas pomposas, que abusa de DLLs e Namespaces para simplesmente fazer a Solution parecer mais complexa, luxuosa, mais elegante ou mais importante do que realmente é. Porque certamente depois de um tempo isso vai ser apenas ruído nos projetos.
Não quero desviar a atenção do problema ou tornar a manutenção difícil, cansativa ou desmotivante até o ponto de se ficar completamente obsoleta ou inviável.
Mas é importante não confundir isso com **super classes** que abraçam mais responsabilidade do que comportam.

Note que a Solution inicial é composta por dois projetos, o Core e o Desktop.
Quando se modela um Domínio, a neutralidade tecnológica é fundamental por isso existe a orientação de um Domínio não ter ligações com Infraestrutura ou Interface de Usuário, pois assim, alterações de tecnologia não influenciem nas regras de negócio.
No entanto, isolar o Domain em uma DLL à parte não é a única forma de fazer. Neste projeto por exemplo, opto por isolar a Interface e manter uma tríade (Domain, Infra e Services) em uma DLL separada, afinal, nesse projeto em específico o Domínio não é tão grande o suficiente e mudanças de Infra são.
Você não verá nenhuma classe do Namespace DomainModel fazendo ```using``` de Namespaces como Infrastructure ou Services.

Todo o isolamento do domínio e a comunicação entre as camadas acontecem normalmente através de modificadores de acesso como ```private``` e ```internal```.

**Domain Driven Design**

Normalmente quando se fala em DDD, já vem a cabeça que é só criar uma Class Library pro Domain, uma pra Infra, uma pra Application e um projeto de GUI pra Presentation e o DDD está feito.
Na verdade, tentarei ir um pouco além disso. Considero que a estrutura de um projeto em DDD dessa forma é pouco expressiva. Quando se pratica a arquitetura propriamente dita você precisa entender o que a solução em palta realmente necessita, e não simplesmente ter um "Template de DDD" que é só renomear e já tem a estrutura.
Ou seja, desde que você compreenda a metodologia e o benefício de cada prática, não precisará e nem deveria usar, uma receita de bolo.

Para esse projeto tentarei utilizar práticas de DDD que lhe são cabíveis, mas não todas.
Tentei aplicar aqui algumas práticas apesar de não termos complexidade o suficiente para vermos níveis mais avançados de arquitetura, mas teremos uma boa iniciação nas suas práticas, pelo menos da **Modelagem do Domínio** e a importância da **Linguagem Ubíqua** nele.

Neste primeiro projeto o BoundedContext é o próprio jogo Tetris, no futuro outros contextos podem ser adicionados para outros jogos ou modalidades.

Outra coisa que dificilmente você não verá nos meus projetos são pastas dentro do Domain uma pasta chamada Entidades outra chamada Value Objects, outra chamada Interfaces, etc. Veja, esse tipo de modelagem está modelando uma solução para a sua arquitetura e não para o Domínio. Entidades, Interfaces e ValueObjects não são coisas que existem para seu cliente, não fazem parte do domínio dele (a não ser que seu cliente seja uma empresa de software).
Novamente, o Domínio precisa ser expressivo, você precisa aprender com ele, falar a lingua dele e utilizar dele para se manter focado no problema (estar no domínio).

Exclusivamente no DomainModel, tento manter ao máximo a linguagem ubíqua, não apenas nomenando os objetos em linguagem natural mas também falando em voz alta e refletindo se o que eu falei faz sentido ou se estava falando com uma máquina! Eventualmente, farei uso de uma licença poética para mesclar português com inglês, principalmente quando estiver lhe dando com Patterns.

**Código Limpo**

Várias práticas do Clean Code de Robert Martin são aplicadas nesse projeto, bons nomes para os objetos e métodos pequenos nem precisam ser citados aqui, mas uma que não é necessariamente do Uncle Bob mas que foi citada por ele é especial aqui **A Lei de Deméter**.
Isso garantirá que o objeto só acesse propriedades e métodos dele mesmo ou de objetos criados por ele.

**Design Pattern**

Tentei aplicar alguns para exemplificá-los na prática, mas ainda deve ter partes a serem refatoradas.
Na primeira publicação veremos pelo menos o uso de State, Strategy, Factory Method, Builder, Bridge, Repository e Controller.

**SOLID**

Assim como no caso dos Design Patterns, é bem possível que achemos partes que violem os princípios SOLID, 
mas mais facilmente veremos boas aplicações de SRP, ISP, DIP (que na minha percepção, pode ser feito sim sem injetores de dependência, por isso é um princípio e não um framework).
Talvez o princípio OCP esteja sendo violado com maior frequência, descubramos!

**Testes**

Infelizmente isso sempre fica por último! Por isso que TDD obriga escrever o teste antes, caso contrário não escreveríamos nunca! Brincadeiras à parte, nesse projeto vai ser diferente, e espero escrevê-los logo.
Inclusive estou usando o MSTest rodando com AxoCover e essa tem sido uma boa experiência.
