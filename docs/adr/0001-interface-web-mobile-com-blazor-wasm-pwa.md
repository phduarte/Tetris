# ADR 0001 — Interface Web e Mobile com Blazor WebAssembly instalável como PWA

- **Status:** Aceito (29/10/2026, com a fase 7: #82, #83, #84 e #85)
- **Data:** 28/10/2026
- **Decisores:** Paulo Duarte
- **Detalhes:** [PLANO-MULTIPLATAFORMA.md](../PLANO-MULTIPLATAFORMA.md)

## Contexto

Um dos objetivos do Gadz.Tetris é ter várias interfaces usando o mesmo `Gadz.Tetris.Domain`. Hoje só existe a interface WinForms (`Gadz.Tetris.Desktop`). Queremos uma interface Mobile e uma Web, de preferência num único projeto que possa ser instalado nos dispositivos como PWA.

Restrições:

- As regras do jogo ficam só no `Domain`, em C# (regras 1 e 2 do `CLAUDE.md`). Uma interface nova não pode reimplementá-las.
- O jogo precisa responder às teclas e toques sem atraso perceptível e, como PWA, funcionar offline.
- O projeto é didático: a solução precisa ser legível e não pode cair em over-engineering.

Bloqueios atuais para rodar fora do Windows:

- O `Board` faz o loop do jogo com `new Thread` + `Thread.Sleep`, o que não funciona no WebAssembly, que roda numa thread só.
- O `GameController` cria o `StatsRepository` (arquivo em disco) e o `ToggleMuteCommand` usa o `SoundPlayerAdapter` (`winmm.dll`). Ou seja, a `Application` depende de infraestrutura do Windows.

## Opções consideradas

1. **Blazor WebAssembly PWA**: UI em Razor/C#, com `Domain` e `Application` rodando no navegador.
2. **React + .NET WebAssembly (`[JSExport]`)**: o `Domain` é compilado para WASM e o React chama ele por uma ponte JavaScript.
3. **React + ASP.NET Core + SignalR**: o jogo roda no servidor e o React só desenha.
4. **Reescrever as regras em TypeScript** e usar React puro.
5. **.NET MAUI nativo**: apps Android/iOS, sem Web.

| Critério | 1. Blazor PWA | 2. React + WASM | 3. React + SignalR | 4. TypeScript | 5. MAUI |
| --- | --- | --- | --- | --- | --- |
| Reusa o `Domain` em C# | ✅ direto | ✅ por ponte JS | ✅ no servidor | ❌ | ✅ direto |
| Web e Mobile num projeto | ✅ | ✅ | ✅ | ✅ | ❌ só Mobile |
| Offline / instalável | ✅ | ✅ | ❌ | ✅ | ✅ |
| Resposta às teclas/toques | local | local | sofre com a rede | local | local |
| Complexidade extra | baixa | alta (interop, serialização de eventos e `Block[,]`) | média (servidor, conexões) | alta (duas bases de regras) | média (lojas, SDKs) |
| Stack | só .NET | .NET + Node/React | .NET + Node/React | Node/React | só .NET |

## Decisão

Adotar a **opção 1**: criar o projeto `Gadz.Tetris.Web` em **Blazor WebAssembly standalone** (`dotnet new blazorwasm --pwa`), responsivo e instalável como PWA. O mesmo projeto atende Web e Mobile.

Para viabilizar a decisão:

1. **Loop fora do `Domain`.** O `Board` expõe um `Tick()` (o atual `Checkout()`), e a `Application` controla o relógio com `PeriodicTimer`/`Task.Delay`, que funcionam no WASM e no Desktop. Ver [#63](https://github.com/phduarte/Tetris/issues/63).
2. **`Application` sem infraestrutura.** Cria-se a interface `ISoundPlayer` na `Application`, e o `GameController.Create` passa a receber `IStatsRepository` e `ISoundPlayer`. A `Application` deixa de referenciar `Data` e `SoundPlayer`. Ver [#53](https://github.com/phduarte/Tetris/issues/53).
3. **Cada interface monta as suas dependências** (composition root):
   - Desktop, no `Program.cs`: `StatsRepository` + `SoundPlayerAdapter`.
   - Web: `LocalStorageStatsRepository` + `WebAudioSoundPlayer` (JS interop), em `Gadz.Tetris.Web/Infrastructure`.
4. **Entradas reaproveitam os `Commands`.** Teclado (`KeyboardEvent.key`) e botões/gestos de toque são convertidos para o enum `Key` e executados pelo `CommandFactory`, sem `switch` na UI.
5. **Textos compartilhados.** `Jogo.resx` e `Jogo-pt.resx` saem do Desktop para um projeto compartilhado e são lidos na Web via `IStringLocalizer`.

Arquitetura resultante:

```
Desktop (WinForms) ─┐
Web (Blazor PWA) ───┼──► Application ──► Domain
                    │
   infra por host:  ├─ Desktop: Data (arquivo) + SoundPlayer (winmm)
                    └─ Web: LocalStorage/IndexedDB + Web Audio
```

## Consequências

### Positivas

- O `Domain` e a `Application` passam a ser usados por duas interfaces sem alterar as regras do jogo. É a demonstração prática do objetivo do projeto.
- Toda a stack continua em .NET: um SDK, um `dotnet build`/`dotnet test` e o mesmo CI.
- Com o loop controlado de fora, os testes de queda, velocidade e nível ficam determinísticos.
- Funciona offline e é instalável em Android, iOS e desktop sem passar pelas lojas.
- Caminho aberto para publicar nas lojas com **.NET MAUI Blazor Hybrid**, que reaproveita os componentes Razor.

### Negativas e riscos

- **Mudam regras do `CLAUDE.md`**: "a `Application` é o único ponto que monta o domínio com a infraestrutura" e "não acesse `Gadz.Tetris.Data` a partir do Desktop". A Desktop passa a referenciar `Data` e `SoundPlayer` só no `Program.cs`. O `CLAUDE.md` precisa ser atualizado junto com a implementação.
- **Download inicial maior** que o de um app JavaScript, por causa do runtime .NET. Mitigação: compressão, trimming e cache do service worker após a primeira visita.
- **Áudio no iOS** só toca depois de uma interação do usuário. Mitigação: tela inicial "Toque para jogar".
- **Recordes não sincronizam entre dispositivos**: cada navegador guarda os seus. Uma API de ranking (`HttpStatsRepository`) fica para uma decisão futura.
- **Menos aderente ao ecossistema React**, caso esse fosse um objetivo de aprendizado. Se React virar requisito, este ADR deve ser substituído por outro que adote a opção 2. As mudanças dos itens 1 a 4 da decisão continuam valendo.

## Referências

- [Plano multiplataforma](../PLANO-MULTIPLATAFORMA.md)
- [Plano de refatoração](../PLANO-REFATORACAO.md): issues #53, #58, #61, #63 e #65
- [Blazor WebAssembly como PWA](https://learn.microsoft.com/aspnet/core/blazor/progressive-web-app)
