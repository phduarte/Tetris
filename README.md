# Tetris

![.NET 4.6](https://github.com/phduarte/Tetris/workflows/.NET%204.6/badge.svg)
[![CodeFactor](https://www.codefactor.io/repository/github/phduarte/tetris/badge)](https://www.codefactor.io/repository/github/phduarte/tetris)
[![BCH compliance](https://bettercodehub.com/edge/badge/phduarte/Tetris?branch=master)](https://bettercodehub.com/)

## O Jogo

  O Jogo foi desenvolvido em C# com propósito único de estudo e prática dos processos de Engenharia de Software.
O projeto utiliza o jogo Tetris como cenário justamente por ser comum para maioria das pessoas, facilitando a correlação das regras no mundo real com os códigos que aqui são escritos.

  Também é propósito desse projeto ser simples. Usar apenas o que agrega valor para a solução e que seja justificadamente necessário. Naturalmente podem haver pontos que se desviam desse propósito, mas assim que percebidos serão refatorados. 

  Notem que a Solution inicial é composta por somente dois projetos principais, o Core e o Desktop.
O Core é responsável pelo backbone do software, provém o Modelo de Domínio e os Acessos a Dados (Que aqui só persiste informações das estatísticas). O projeto Desktop faz parte da camada de Interfaces e é a IHC para desktops Windows. Suas preocupações são apenas nesse escopo - lidar com as entradas vindas dos dispositivos e com as capacidades de respostas visuais mais agradáveis e relevantes possível ao usuário.

  Enquanto eu modelava o Modelo de Domínio, a neutralidade tecnológica e o desacoplamento das responsabilidades foi algo fundamental, por isso o Domínio não tem ligações externas com camadas de Infraestrutura ou Interface por exemplo. Afinal, não podemos permitir que regras do jogo sejam manipuladas pelo usuário, e nem ficar preso num modelo de Interface só. Provavelmente teremos versões para outros dispositivos de IHC, como Mobile por exemplo. No entanto, não isolei o Domain Model em uma DLL à parte (simplesmente porque não faz nenhum sentido pra mim fazer isso nesse projeto), mas isolei todas as classes, métodos e propriedades que precisavam simplesmente usando operadores de visibilidade *Internal* e *Private*.

## Características

- **Tema Clássico**: Opção para selecionar o tema similar ao do jogo original da década de 80.
- **Tema Brickgame**: Opção para selecionar o tema similar ao do video-game de bolso, famoso na década de 90.
- **Slide**: Segurar a tecla CTRL enquanto move a peça para esquerda ou direita acelera a velocidade do movimento.
- **Smash Down**: Ao apertar a tecla SPACEBAR a peça imediatamente cai em velocidade acelerada.
- **Sons**: Os eventos e movimentos reproduzem sons similares aos dos brickgames.
- **Sound On/Off**: Opção para ligar ou desligar sons na tela inicial ou durante o jogo pressionando a tecla SHIFT.
- **Pontuação**:
  + 1 linha completa vale 40 pontos
  + 2 linhas completas na mesma jogada valem 100 pontos
  + 3 linhas completas na mesma jogada valem 300 pontos
  + TETRIS (4 linhas completas na mesma jogada) vale 1.200 pontos

## Gráfico

![img1](doc/tela2.png)

## Links úteis

- [Baixar](Tetris.zip)
- [Ferramentas usadas](doc/ferramentas.md)
