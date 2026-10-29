# Instruções do projeto Gadz.Tetris

Jogo Tetris para Windows (WinForms) feito como estudo de DDD, Clean Code, SOLID e Design Patterns.
O objetivo do projeto é ser didático: código legível e bem separado vale mais que código curto.
Ao aplicar um padrão, avalie custo-benefício — **evite over-engineering**.

## Stack

- .NET 10 (`net10.0`; o Desktop usa `net10.0-windows` + WinForms). SDK fixado em `global.json`.
- Configuração comum (TargetFramework, `LangVersion latest`, `Nullable`, `ImplicitUsings`, análise de código) fica em `Directory.Build.props` — não repita essas propriedades nos `.csproj`.
- Estilo de código definido em `.editorconfig` (`dotnet format` aplica).
- Testes: NUnit (modelo clássico `Assert.AreEqual`, `Assert.IsTrue`...).
- Som via `winmm.dll` (`mciSendString`) — só funciona no Windows.

```sh
dotnet build Gadz.Tetris.sln
dotnet test Gadz.Tetris.sln
dotnet run --project Gadz.Tetris.Desktop
```

CI: `.github/workflows/main.yml` (restore, build e test em `windows-latest`). O `appveyor.yml` faz o mesmo, instalando antes o SDK do `global.json` (a imagem do AppVeyor não o inclui).

## Arquitetura (camadas e dependências)

```
Desktop ──► Application ──► Domain
   │                   └──► Data ──► Domain
   └──► SoundPlayer ──► Application (implementa ISoundPlayer)
```

| Projeto | Responsabilidade |
|---|---|
| `Gadz.Tetris.Domain` | Modelo de domínio e **todas** as regras do jogo (tabuleiro, peças, colisão, pontuação, níveis, estados). Não referencia nenhum outro projeto nem pacote. |
| `Gadz.Tetris.Data` | Implementação dos repositórios do domínio (`StatsRepository` → arquivo texto `%USERPROFILE%\Tetris\stats.mc`). |
| `Gadz.Tetris.SoundPlayer` | Adapter de áudio (`SoundPlayerAdapter : ISoundPlayer`) com os `.wav` embutidos em `Sounds.resx`. Criado uma única vez no `Program.cs` do Desktop. |
| `Gadz.Tetris.Application` | `GameController` (fachada para o cliente) e os `Commands` de teclado. É o único ponto que monta o domínio com a infraestrutura. |
| `Gadz.Tetris.Desktop` | Interface WinForms: captura entradas e desenha a tela. Sem regras de jogo. Um único form sem borda (`Device/ConsoleFrame`, o "aparelho") desenha carcaça, LCD e botões; as telas (`Screens/*Screen`) só trocam o conteúdo do LCD. |
| `Gadz.Tetris.Tests` | Testes NUnit do domínio e do `GameController`. |

## Padrões de projeto usados (siga-os ao estender)

- **State** — `IBoardState` com `PlayingState`, `PausedState`, `FinishedState` (`internal`, instância única em `Instance`). Permissões (`CanMove`, `CanPause`...) são consultadas no estado, não em `if`s espalhados.
- **Builder** — `PieceBuilder` (`OfType().OnPosition().WithRotation().Build()`) é a única forma de criar peças.
- **Strategy** — cada tetraminó (`TetraminoI/J/L/O/S/T/Z`, `internal`) declara sua forma como dado: `SHAPES` (rotação 0–3 → 4 offsets `Point`). A classe base `Piece` monta os `Blocks` a partir dela.
- **Gerador de peças** — `IPieceGenerator` (domínio) escolhe tipo e rotação de cada nova peça; o `Board` usa o `RandomPieceGenerator` (`internal`) por padrão e aceita outro pelo construtor. Nos testes, use o `SequencePieceGenerator` para cenários determinísticos.
- **Repository** — interface no domínio (`IStatsRepository : IRepository`), implementação em `Data`.
- **Command** — cada ação de teclado é uma subclasse de `Command` (`Key`, `Control`, `Execute()`) registrada em `CommandFactory` (respeita OCP).
- **Facade / Factory Method** — `GameController.Create(width, height)` com construtor privado.
- **Adapter** — `SoundPlayerAdapter` encapsula a API nativa de áudio atrás do `ISoundPlayer` (interface na `Application`, porque som não é regra de jogo). O `GameController` recebe o `ISoundPlayer` no `Create` e liga os sons aos eventos do `Board`; as telas usam a mesma instância por `ConsoleFrame.Sound` (som fora da partida, mudo, música de fim).
- **Eventos** — notificações usam o delegate `GameActionEventHandler` e eventos `On*` (`OnRefresh`, `OnClear`, `OnMove`, `OnDrop`, `OnLose`...), disparados com `Evento?.Invoke()`. O `GameController` repassa os eventos do `Board`.
- **Telas do aparelho (Desktop)** — o `ConsoleFrame` exibe uma `ConsoleScreen` por vez (`MenuScreen`, `PlayScreen`, `ResultScreen`, `HelpScreen`, `AboutScreen`) e troca com `Navigate`. Cada tela declara o `ConsoleLayout` (direcional, botões pequenos e botão grande, cada um com tecla e legenda), desenha o LCD via `LcdCanvas` (células, 7 segmentos, textos) e trata as teclas. Clicar num botão equivale a apertar a tecla dele. Pausa e GAME OVER são modos de desenho da `PlayScreen`, não forms.

## Convenções de código

- **Namespaces** espelham as pastas: `Gadz.Tetris.<Projeto>.<Pasta>` (ex.: `Gadz.Tetris.Domain.Models.Pieces`). Use namespace file-scoped (`namespace X;`).
- **C# moderno**: prefira primary constructors, switch expressions, collection expressions (`[]`), `new()` com tipo inferido e `using` declarations. Nada de avisos de nullable: anote com `?` o que pode ser nulo em vez de suprimir.
- **Nomes**: `PascalCase` para tipos/membros públicos, `_camelCase` para campos privados, `UPPER_SNAKE_CASE` para constantes (`MAX_LINES_PER_DROP`, `BLOCK_SIZE`).
- **Idioma**: identificadores novos em inglês (padrão adotado na refatoração recente). Existem nomes legados em português (`ColideCom`) — não renomeie fora de uma refatoração dedicada. Documentação XML voltada ao cliente (`GameController`) e mensagens de commit costumam estar em português.
- **Testes**: classes em português terminando em `Tests` (`PecaTests`, `EstatisticasTests`), métodos no formato `DeveXxx` / `NaoDeveXxx`.
- **Documentação**: membros públicos e privados levam `/// <summary>`. Comentários de linha só para explicar o *porquê* de algo não óbvio.
- **Encapsulamento**: propriedades de domínio com `private set`; coleções expostas como `IReadOnlyList`/`IEnumerable`; tipos que não precisam ser vistos fora do assembly são `internal`.
- **Value objects** (`Point`, `Size`, `Block`, `Identity`, `StatsRecord`, `PieceConfiguration`) são `struct`; **entidades** herdam de `Entity` (possuem `Identity Id`).
- **Async na UI**: métodos de UI assíncronos terminam em `Async`.
- Arquivos `*.Designer.cs`, `Resources.Designer.cs`, `Sounds.Designer.cs` e `Jogo*.Designer.cs` são gerados — altere o `.resx`/designer do Visual Studio, não o arquivo gerado.

## Regras do jogo (implementadas no domínio)

- Tabuleiro padrão 10×20; `GameController.Create` rejeita largura ou altura < 10.
- 7 peças com cores fixas: I=Cyan, T=Purple, L=Orange, J=Blue, O=Yellow, S=Green, Z=Red. Rotação cicla 0→3→0.
- Pontuação por jogada: 1 linha = 40, 2 = 100, 3 = 300, 4 (TETRIS) = 1200. No máximo 4 linhas por jogada.
- Nível sobe ao completar a meta de linhas (inicia em 20 e cresce 25% a cada nível).
- Velocidade: a peça desce a cada `Stats.Interval` ms — 1000 no nível 1, menos 50 por nível, mínimo de 100. O domínio não tem relógio: cada passo é um `Board.Tick()`, chamado pelo `GameController` (`Task.Delay`, sem threads próprias, para rodar também no WebAssembly). Nos testes de domínio, chame `Tick()` em vez de esperar.
- Recorde (HI-SCORE): `Board.Record` é a maior pontuação guardada no repositório ou a pontuação atual, se ela já a superou; `Board.IsNewRecord` diz se a partida superou o recorde guardado. As estatísticas são salvas ao sair (`Exit`) e ao perder (o `GameController` salva antes de repassar o `OnLose`).
- A tabela de pontuação é exposta como `Stats.ScoreTable` (via `GameController.ScoreTable`) para a ajuda não repetir os números.
- Tempo jogado: contado pelo `PlayClock` (`internal`) dentro de `Stats`, sem contar as pausas.
- Estados: `Playing` ↔ `Parado` (pausado) → `Finished`. Os textos de `ToString()` dos estados são verificados nos testes.
- Teclas: ←/→ movem (segurando, `HoldLeft/HoldRightCommand` andam 2 blocos a cada repetição automática da tecla; o primeiro toque anda 1), Ctrl+←/→ deslizam até o limite, ↓ desce, Espaço derruba (smash down), ↑ gira, Enter pausa, Esc encerra o jogo e salva as estatísticas, Shift liga/desliga som.

## ⛔ Regras que não podem ser quebradas

1. **O domínio é independente.** `Gadz.Tetris.Domain` não pode referenciar nenhum outro projeto da solução, WinForms, arquivos, áudio ou pacotes externos. Infraestrutura entra apenas por interfaces definidas no domínio (ex.: `IStatsRepository`).
2. **Regras de jogo só no domínio.** Colisão, movimento, pontuação, níveis, limpeza de linhas e transições de estado ficam em `Domain`. `Application` apenas orquestra; `Desktop` apenas lê entradas e desenha. Nunca calcule pontuação ou valide movimento na UI.
3. **O Desktop fala com o jogo via `GameController` e `Commands`.** Não instancie `Board`, peças ou repositórios na UI, e não acesse `Gadz.Tetris.Data` a partir do Desktop.
4. **Novas ações de teclado = nova subclasse de `Command`** registrada em `CommandFactory`. Não adicione `switch`/`if` de teclas nos formulários. Teclas de navegação das telas fora da partida (menu, ajuda, sobre, resultado), que não agem sobre o jogo, ficam na tabela `KeyBindings` da `ConsoleScreen`, também sem `switch`.
5. **Peças só são criadas pelo `PieceBuilder`.** Novos tipos de peça = novo `PieceType` + nova classe `Tetramino*` `internal` + `case` no `Build()` + cor em `Piece._colors`.
6. **Estado do tabuleiro só muda via State.** Não crie flags booleanas paralelas para "jogando/pausado"; adicione permissões em `IBoardState` e implemente em todos os estados.
7. **Não exponha estado mutável do domínio.** Mantenha `private set`, coleções somente leitura e tipos internos como `internal`.
8. **Sem números mágicos.** Valores com significado viram constantes nomeadas (`private const`).
9. **Textos de interface ficam em recursos.** Todo texto visível vai em `Textos/Jogo.resx` (inglês) **e** `Textos/Jogo.pt.resx` (português), com a mesma chave nos dois.
10. **Atualizações de controles WinForms acontecem na thread de UI.** O relógio do `GameController` (`RunClockAsync`) chama `Board.Tick` fora da thread de UI; use o `TaskScheduler` capturado (`_threadPrincipal`) como em `PlayScreen.RunOnUiAsync`.
11. **Compatibilidade do arquivo de estatísticas.** `stats.mc` é texto separado por tab com ordem de colunas fixa (`COL_ID`…`COL_TIME`). Só acrescente colunas ao final e mantenha a leitura de arquivos antigos funcionando.
12. **Testes acompanham as regras.** Toda regra de domínio nova ou alterada precisa de teste em `Gadz.Tetris.Tests`. Mudanças na tabela de pontuação, cores, estados ou teclas exigem atualizar testes **e** o `README.md`.
13. **Não edite arquivos gerados** (`*.Designer.cs`) à mão.
14. **Testes passando antes de abrir ou atualizar uma PR.** Rode `dotnet test Gadz.Tetris.sln` e só abra a PR (ou faça push nela) com todos os testes verdes. Teste novo ou alterado que dependa de sorteio, tempo ou thread deve ser executado várias vezes seguidas (ex.: 20× com `--filter`) antes do push — uma única execução local não revela falha intermitente, e a CI (GitHub Actions e AppVeyor) roda de novo a cada push.

## Armadilhas conhecidas

- O `GameController` sorteia as peças (o `Create` público não aceita outro gerador; há uma sobrecarga `internal` com `IPieceGenerator`, visível só para `Gadz.Tetris.Tests`). Teste que posicione a peça deve valer para qualquer tipo e rotação: a I deitada ocupa 4 colunas, e `CurrentPiecePosition` é a origem da forma, que pode ficar fora do tabuleiro (ex.: -1) quando os blocos começam numa coluna interna. Para conferir a borda, use os blocos (`GetActualBlocks().Min(b => b.X)`); para cenários exatos, teste o `Board` com o `SequencePieceGenerator`.
- `GameController.Create(width, height, null, sound)` usa o `StatsRepository` real (`%USERPROFILE%\Tetris\stats.mc`). Nos testes, sempre passe o `InMemoryStatsRepository` e o `FakeSoundPlayer` (`Gadz.Tetris.Tests.Model`) para não gravar em disco nem tocar som.
- `PlayScreen.HandleKey` executa só o primeiro comando que casar com a tecla (ordem do `CommandFactory`); por isso cada tecla deve ter um único comando — o Enter usa `TogglePauseCommand`, que consulta `CanPause`/`CanContinue` no estado. `Command.Match` recebe também `repeat` (tecla segurada, detectada pelo `ConsoleFrame`): por padrão o comando ignora esse dado; `MoveLeft/Right` só casam no primeiro toque e `HoldLeft/Right` só nas repetições.
- `Key` usa os códigos do WinForms (`Keys`): o Shift chega como `Keys.ShiftKey` (0x10), não `LShiftKey`.
- `SoundPlayerAdapter` e `StatsRepository` falam com o sistema (winmm, disco) e estão marcados com `[ExcludeFromCodeCoverage]`. Crie um só `SoundPlayerAdapter` por aplicação: o construtor abre os sons com aliases fixos.
- A UI lê os textos pela classe gerada de `Textos/Jogo.resx` (`using Texto = ...Textos.Jogo`). O `Jogo.pt.resx` é um recurso satélite (não gera classe): o Windows em português recebe os textos dele automaticamente.
- Sem o Visual Studio, a classe de um `.resx` pode ser regenerada com o `ResGen.exe /str:cs,...` do Windows SDK. Ele escreve os comentários no idioma do sistema; traduza-os de volta para o texto padrão em inglês (ou copie só o membro novo para o `Jogo.Designer.cs`).
- As telas não têm designer: tudo é desenhado em código, em pixels a 96 DPI, e o `ConsoleFrame` escala para o DPI do monitor. Medidas e cores ficam em constantes e na `Palette`; a fonte do LCD (Silkscreen, OFL) e o ícone estão em `Properties/Resources.resx`.
- O ícone (`tetris.ico`) é gerado por `dotnet run tools/gerar-icone.cs -- Gadz.Tetris.Desktop/tetris.ico`; altere o script, não o `.ico`.
