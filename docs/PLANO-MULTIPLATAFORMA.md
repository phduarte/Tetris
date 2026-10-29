# Plano multiplataforma: Web e Mobile — Gadz.Tetris

Análise de 28/10/2026, branch `main`.

Um dos objetivos do projeto é ter N interfaces usando o mesmo `Domain`. Hoje só existe a interface WinForms. Este documento propõe uma interface única para Web e Mobile, instalável como PWA, e lista o que precisa mudar antes disso. Algumas mudanças já têm issue no [plano de refatoração](PLANO-REFATORACAO.md) e estão indicadas abaixo.

## A decisão principal: React ou Blazor

Um app React em JavaScript não executa o `Domain` em C#. As alternativas:

| Opção | Como funciona | Reusa o Domain? | Offline/PWA | Veredito |
| --- | --- | --- | --- | --- |
| **A. Blazor WebAssembly PWA** | UI em Razor/C#. `Domain` e `Application` rodam no navegador | ✅ sem mudanças | ✅ (template `--pwa`) | **Recomendada** |
| B. React + .NET WASM (`[JSExport]`) | O `Domain` é compilado para WASM e o React chama ele por uma ponte JS | ✅ | ✅ | Funciona, mas a ponte JS↔C# (eventos, `Block[,]`, serialização) dá bastante trabalho extra |
| C. React + ASP.NET Core + SignalR | O jogo roda no servidor e o React só desenha | ✅ | ❌ | Não serve como base: a latência atrapalha o controle das peças e não funciona offline |
| D. Reescrever as regras em TypeScript | — | ❌ | ✅ | Vai contra o objetivo do projeto |

**Recomendação: opção A.** Um único projeto `Gadz.Tetris.Web` em Blazor WASM, instalável como PWA no desktop, no Android e no iOS. Ele cobre Web e Mobile ao mesmo tempo e mostra bem a ideia de "N interfaces, um domínio". Para publicar nas lojas no futuro, o **.NET MAUI Blazor Hybrid** reaproveita os mesmos componentes Razor.

Se React for requisito, a opção B é a única que preserva o objetivo. O plano abaixo vale para as duas; só a fase 1 muda.

## Fase 0 — Application independente de plataforma (obrigatória)

Hoje existem três bloqueios para rodar fora do Windows.

### 1. O loop do jogo usa `new Thread` + `Thread.Sleep`

O `Board.RefreshAsync()` cria uma thread de background. O WebAssembly roda numa thread só, então isso não funciona lá.

- O `Domain` expõe um `Tick()` (o atual `Checkout()`), e o "relógio" fica fora dele.
- A `Application` controla o loop com `PeriodicTimer`/`Task.Delay`, que funcionam no WASM.
- Bônus: os testes de queda e de nível ficam determinísticos, porque dá para chamar `Tick()` direto sem esperar tempo real.
- Relacionada: [#63](https://github.com/phduarte/tetris/issues/63) (serializar as mutações do `Board`). ✅ Fechada com um `lock` no `Board`.
- ✅ [#82](https://github.com/phduarte/tetris/issues/82): o `Board` expõe `Tick()` e o `GameController` desce a peça com `Task.Delay`, lendo `Stats.Interval` a cada volta.

### 2. A Application está presa à infraestrutura do Windows

- ~~`GameController.Create` faz `new StatsRepository()`, que grava em arquivo.~~ ✅ [#84](https://github.com/phduarte/tetris/issues/84): `Create(width, height, IStatsRepository, ISoundPlayer)`; a `Application` referencia só o `Domain`.
- ~~`ToggleMuteCommand` chama o `SoundPlayerAdapter` estático, que usa `winmm.dll`.~~ ✅ [#83](https://github.com/phduarte/tetris/issues/83): `ISoundPlayer` na `Application`; o Desktop cria o `SoundPlayerAdapter` e o passa ao `Create`.

Proposta: criar a interface `ISoundPlayer` na `Application` (som não é regra de jogo, então não vai no `Domain`) e mudar a assinatura para `GameController.Create(width, height, IStatsRepository, ISoundPlayer)`. A `Application` deixa de referenciar `Data` e `SoundPlayer`.

### 3. A montagem das dependências passa para cada interface

| Interface | Repositório | Som |
| --- | --- | --- |
| Desktop | `StatsRepository` (arquivo `stats.mc`) | `SoundPlayerAdapter` (winmm) |
| Web | `LocalStorageStatsRepository` (localStorage/IndexedDB) | `WebAudioSoundPlayer` (JS interop) |

✅ **Decidido (#84).** O Desktop referencia `Data` e `SoundPlayer` **só para montar as dependências**, no `Program.cs`. O `CLAUDE.md` foi atualizado (diagrama, tabela de projetos e regra 3).

### Ajustes menores

| Item | Proposta | Relacionada |
| --- | --- | --- |
| Teclas | O enum `Key` usa códigos de tecla do Windows. A Web mapeia o `KeyboardEvent.key` do navegador para `Key`, e os botões de toque disparam os mesmos `Key`, reaproveitando o `CommandFactory` sem `switch` | — |
| Pausa no Enter | Unificar `PauseCommand`/`ContinueCommand` em `TogglePauseCommand` que consulta o State | [#58](https://github.com/phduarte/tetris/issues/58) ✅ |
| Textos | ✅ `Jogo.resx` e `Jogo.pt.resx` no projeto compartilhado `Gadz.Tetris.Texts`; a Web usará as mesmas chaves via `IStringLocalizer` | [#85](https://github.com/phduarte/tetris/issues/85) |
| `Matrix` | Expor uma cópia somente leitura (snapshot) em vez de `Block[,]` mutável | [#61](https://github.com/phduarte/tetris/issues/61) ✅ |
| `Board` exposto | O `GameController` não deve expor a entidade `Board` às interfaces | [#65](https://github.com/phduarte/tetris/issues/65) ✅ |

Ao fim da fase 0, o Desktop funciona igual e os testes cobrem as mudanças.

## Fase 1 — Web MVP

```
Desktop (WinForms) ─┐
Web (Blazor PWA) ───┼──► Application ──► Domain
                    │
   infra por host:  ├─ Desktop: Data (arquivo) + SoundPlayer (winmm)
                    └─ Web: LocalStorage/IndexedDB + Web Audio (JS interop)
```

- Criar com `dotnet new blazorwasm --pwa -o Gadz.Tetris.Web` e adicionar à solução.
- Pasta `Infrastructure/` com o `LocalStorageStatsRepository` e o `WebAudioSoundPlayer`, que toca os mesmos `.wav` colocados em `wwwroot`.
- Desenho do tabuleiro: um grid CSS de 10×20 `div`s resolve e é didático. Canvas só se aparecer problema de desempenho.
- Os eventos `On*` chegam pelo timer, então a tela atualiza com `InvokeAsync(StateHasChanged)`, o equivalente ao `_threadPrincipal` de `Play.cs`.

## Fase 2 — PWA e mobile

- Ajustar `manifest.webmanifest` (ícones, `display: standalone`, `orientation: portrait`) e o service worker para cache offline.
- Layout responsivo com botões na tela (←, →, ↻, ↓, ⤓) e gestos de deslizar.
- Opcional: vibração ao limpar linhas (`navigator.vibrate`).
- No iOS, o áudio só toca depois de um primeiro toque do usuário. Basta uma tela "Toque para jogar".

## Fase 3 — Deploy e CI

- Adicionar no `.github/workflows/main.yml` um job que publica a Web no **GitHub Pages** (ou Azure Static Web Apps, ambos gratuitos), ajustando o `base href`.
- Os testes seguem rodando contra a `Application`, que agora vale para todas as interfaces.

## Extras para depois

- **MAUI Blazor Hybrid** para publicar nas lojas, reaproveitando os componentes Razor.
- **API de ranking online** em ASP.NET Core: um `HttpStatsRepository` seria só mais uma implementação de `IStatsRepository`, um bom exemplo didático do Repository.
