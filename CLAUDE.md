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
                       ├──► Data ──► Domain
                       └──► SoundPlayer
```

| Projeto | Responsabilidade |
|---|---|
| `Gadz.Tetris.Domain` | Modelo de domínio e **todas** as regras do jogo (tabuleiro, peças, colisão, pontuação, níveis, estados). Não referencia nenhum outro projeto nem pacote. |
| `Gadz.Tetris.Data` | Implementação dos repositórios do domínio (`StatsRepository` → arquivo texto `%USERPROFILE%\Tetris\stats.mc`). |
| `Gadz.Tetris.SoundPlayer` | Adapter de áudio (`SoundPlayerAdapter`) com os `.wav` embutidos em `Sounds.resx`. |
| `Gadz.Tetris.Application` | `GameController` (fachada para o cliente) e os `Commands` de teclado. É o único ponto que monta o domínio com a infraestrutura. |
| `Gadz.Tetris.Desktop` | Interface WinForms: captura entradas e desenha a tela. Sem regras de jogo. |
| `Gadz.Tetris.Tests` | Testes NUnit do domínio e do `GameController`. |

## Padrões de projeto usados (siga-os ao estender)

- **State** — `IBoardState` com `PlayingState`, `PausedState`, `FinishedState` (`internal`). Permissões (`CanMove`, `CanPause`...) são consultadas no estado, não em `if`s espalhados.
- **Builder** — `PieceBuilder` (`OfType().OnPosition().WithRotation().OnBoard().Build()`) é a única forma de criar peças.
- **Strategy** — cada tetraminó (`TetraminoI/J/L/O/S/T/Z`, `internal`) define seus `Blocks` para as rotações 0–3.
- **Repository** — interface no domínio (`IStatsRepository : IRepository`), implementação em `Data`.
- **Command** — cada ação de teclado é uma subclasse de `Command` (`Key`, `Control`, `Execute()`) registrada em `CommandFactory` (respeita OCP).
- **Facade / Factory Method** — `GameController.Create(width, height)` com construtor privado.
- **Adapter** — `SoundPlayerAdapter` encapsula a API nativa de áudio.
- **Eventos** — notificações usam o delegate `GameActionEventHandler` e eventos `On*` (`OnRefresh`, `OnClear`, `OnMove`, `OnDrop`, `OnLose`...), disparados com `Evento?.Invoke()`. O `GameController` repassa os eventos do `Board`.

## Convenções de código

- **Namespaces** espelham as pastas: `Gadz.Tetris.<Projeto>.<Pasta>` (ex.: `Gadz.Tetris.Domain.Models.Pieces`). Use namespace file-scoped (`namespace X;`).
- **C# moderno**: prefira primary constructors, switch expressions, collection expressions (`[]`), `new()` com tipo inferido e `using` declarations. Nada de avisos de nullable: anote com `?` o que pode ser nulo em vez de suprimir.
- **Nomes**: `PascalCase` para tipos/membros públicos, `_camelCase` para campos privados, `UPPER_SNAKE_CASE` para constantes (`MAXIMO_LINHAS_POSSIVEIS`, `BLOCK_SIZE`).
- **Idioma**: identificadores novos em inglês (padrão adotado na refatoração recente). Existem nomes legados em português (`TrocarPecaAtual`, `ColideCom`) — não renomeie fora de uma refatoração dedicada. Documentação XML voltada ao cliente (`GameController`) e mensagens de commit costumam estar em português.
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
- Velocidade: a peça desce a cada `Stats.Interval` ms — 1000 no nível 1, menos 50 por nível, mínimo de 100.
- Estados: `Playing` ↔ `Parado` (pausado) → `Finished`. Os textos de `ToString()` dos estados são verificados nos testes.
- Teclas: ←/→ movem, Ctrl+←/→ deslizam até o limite, ↓ desce, Espaço derruba (smash down), ↑ gira, Enter pausa, Esc encerra o jogo e salva as estatísticas, Shift liga/desliga som.

## ⛔ Regras que não podem ser quebradas

1. **O domínio é independente.** `Gadz.Tetris.Domain` não pode referenciar nenhum outro projeto da solução, WinForms, arquivos, áudio ou pacotes externos. Infraestrutura entra apenas por interfaces definidas no domínio (ex.: `IStatsRepository`).
2. **Regras de jogo só no domínio.** Colisão, movimento, pontuação, níveis, limpeza de linhas e transições de estado ficam em `Domain`. `Application` apenas orquestra; `Desktop` apenas lê entradas e desenha. Nunca calcule pontuação ou valide movimento na UI.
3. **O Desktop fala com o jogo via `GameController` e `Commands`.** Não instancie `Board`, peças ou repositórios na UI, e não acesse `Gadz.Tetris.Data` a partir do Desktop.
4. **Novas ações de teclado = nova subclasse de `Command`** registrada em `CommandFactory`. Não adicione `switch`/`if` de teclas nos formulários.
5. **Peças só são criadas pelo `PieceBuilder`.** Novos tipos de peça = novo `PieceType` + nova classe `Tetramino*` `internal` + `case` no `Build()` + cor em `Piece._colors`.
6. **Estado do tabuleiro só muda via State.** Não crie flags booleanas paralelas para "jogando/pausado"; adicione permissões em `IBoardState` e implemente em todos os estados.
7. **Não exponha estado mutável do domínio.** Mantenha `private set`, coleções somente leitura e tipos internos como `internal`.
8. **Sem números mágicos.** Valores com significado viram constantes nomeadas (`private const`).
9. **Textos de interface ficam em recursos.** Todo texto visível vai em `Textos/Jogo.resx` (inglês) **e** `Textos/Jogo-pt.resx` (português), com a mesma chave nos dois.
10. **Atualizações de controles WinForms acontecem na thread de UI.** O `Board` roda o loop em thread de background; use o `TaskScheduler` capturado (`_threadPrincipal`) como em `Play.cs`.
11. **Compatibilidade do arquivo de estatísticas.** `stats.mc` é texto separado por tab com ordem de colunas fixa (`COL_ID`…`COL_TIME`). Só acrescente colunas ao final e mantenha a leitura de arquivos antigos funcionando.
12. **Testes acompanham as regras.** Toda regra de domínio nova ou alterada precisa de teste em `Gadz.Tetris.Tests`. Mudanças na tabela de pontuação, cores, estados ou teclas exigem atualizar testes **e** o `README.md`.
13. **Não edite arquivos gerados** (`*.Designer.cs`) à mão.

## Armadilhas conhecidas

- `GameController.Create(width, height)` sem repositório usa o `StatsRepository` real (`%USERPROFILE%\Tetris\stats.mc`). Nos testes, sempre passe o `InMemoryStatsRepository` (`Gadz.Tetris.Tests.Model`) para não gravar em disco.
- `Play.Jogo_KeyDown` executa só o primeiro comando que casar com a tecla (ordem do `CommandFactory`); por isso cada tecla deve ter um único comando — o Enter usa `TogglePauseCommand`, que consulta `CanPause`/`CanContinue` no estado.
- `SoundPlayerAdapter` e `StatsRepository` são estáticos/têm construtor estático e estão marcados com `[ExcludeFromCodeCoverage]`.
