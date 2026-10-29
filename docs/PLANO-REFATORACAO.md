# Plano de refatoração — Gadz.Tetris

Análise de 28/10/2026, branch `modernize/dotnet10`.

A arquitetura em camadas está sólida. Os problemas se concentram no `Board` (730 linhas) e na interação entre a thread do jogo e a UI. São 20 itens, cada um com uma issue no GitHub, organizados em 5 fases.

> **Situação:** as fases 1 a 5 estão concluídas (todas as 20 issues fechadas). A fase 6 reúne itens antigos do backlog; veja o fim do documento.

## Como ler as labels das issues

| Grupo | Labels | Significado |
| --- | --- | --- |
| Importância | `prioridade:alta`, `prioridade:média`, `prioridade:baixa` | Alta = bug visível ou bloqueia outros itens; baixa = limpeza |
| Ordem | `fase-1` … `fase-6` | Sequência sugerida; cada fase destrava a seguinte |
| Validação | `validação:teste-unitário` | Exige teste NUnit novo que falha antes e passa depois |
| Validação | `validação:regressão` | Sem mudança de comportamento: `dotnet test` continua verde |
| Validação | `validação:manual` | Jogar e observar o comportamento descrito na issue |
| Camada | `camada:domain`, `camada:application`, `camada:desktop` | Onde a mudança acontece |
| Tipo | `bug`, `refactoring` | Labels já existentes no repositório |

Filtros úteis no GitHub: `is:open label:fase-1`, `is:open label:prioridade:alta`.

## Fase 1 — Fundação de testes

Sem isso, os testes dependem do arquivo `%USERPROFILE%\Tetris\stats.mc` e do `Random`, e os bugs da fase 2 não podem ser reproduzidos de forma determinística.

| Issue | Item | Prioridade | Validação |
| --- | --- | --- | --- |
| [#53](https://github.com/phduarte/tetris/issues/53) | Injetar `IStatsRepository` no `GameController` (remove o `--filter` do CI) | alta | teste-unitário |
| [#54](https://github.com/phduarte/tetris/issues/54) | Injetar o gerador de peças aleatórias no `Board` | média | teste-unitário |

## Fase 2 — Bugs de regra de jogo

Cada bug deve ganhar primeiro um teste que falha (regra 12 do `CLAUDE.md`).

| Issue | Item | Prioridade | Validação |
| --- | --- | --- | --- |
| [#55](https://github.com/phduarte/tetris/issues/55) | `Rotate()` testa a colisão da peça **antes** de girar e permite sobrepor blocos | alta | teste-unitário |
| [#48](https://github.com/phduarte/tetris/issues/48) | Velocidade quase não muda: o intervalo cai de 999 ms para 998 ms por nível | alta | teste-unitário, manual |
| [#56](https://github.com/phduarte/tetris/issues/56) | `Checkout()` não reseta `_refreshing` após `Lose()`; `Restart()` trava o jogo | alta | teste-unitário |
| [#57](https://github.com/phduarte/tetris/issues/57) | `ThereIsCollision` ignora a checagem com até 1 bloco assentado | média | teste-unitário |
| [#58](https://github.com/phduarte/tetris/issues/58) | `Pause`/`Continue` ignoram o estado; unificar Enter em `TogglePauseCommand` | média | teste-unitário, manual |

## Fase 3 — Decomposição do `Board`

| Issue | Item | Prioridade | Validação |
| --- | --- | --- | --- |
| [#59](https://github.com/phduarte/tetris/issues/59) | Unificar `CanMoveLeft/Right/Down` em `Fits` + `TryMove` | média | teste-unitário |
| [#60](https://github.com/phduarte/tetris/issues/60) | Extrair `Playfield` e renomear os métodos em português | média | regressão |
| [#61](https://github.com/phduarte/tetris/issues/61) | Não expor `Matrix` como `Block[,]` mutável | média | regressão |
| [#62](https://github.com/phduarte/tetris/issues/62) | Estados do `Board` como instâncias únicas | baixa | regressão |

## Fase 4 — Concorrência entre o loop e a UI

O loop do `Board` roda numa `Thread` de background e os comandos de teclado rodam na thread da UI. Ambos alteram `CurrentPiece` e os blocos assentados sem sincronização.

| Issue | Item | Prioridade | Validação |
| --- | --- | --- | --- |
| [#63](https://github.com/phduarte/tetris/issues/63) | Serializar as mutações do `Board` (`lock` ou `Tick()` chamado de fora) | alta | teste-unitário, manual |
| [#64](https://github.com/phduarte/tetris/issues/64) | `PaintNextPieceAsync` altera controles fora da thread da UI | alta | manual |

As duas têm prioridade alta, mas ficam na fase 4 porque #63 fica muito mais simples depois da fase 3. A #64 é uma correção de uma linha e pode ser feita a qualquer momento.

## Fase 5 — Fronteiras entre camadas e limpeza

| Issue | Item | Prioridade | Validação |
| --- | --- | --- | --- |
| [#65](https://github.com/phduarte/tetris/issues/65) | `GameController` não deve expor a entidade `Board` | média | regressão |
| [#66](https://github.com/phduarte/tetris/issues/66) | Remover a dependência `Piece` → `Board` | média | regressão |
| [#68](https://github.com/phduarte/tetris/issues/68) | Extrair os números mágicos da pontuação e dos níveis | média | regressão |
| [#67](https://github.com/phduarte/tetris/issues/67) | Formas dos tetraminós como tabela de offsets | baixa | regressão |
| [#69](https://github.com/phduarte/tetris/issues/69) | Trocar os comentários XML gerados por documentação útil | baixa | regressão |
| [#70](https://github.com/phduarte/tetris/issues/70) | `CommandFactory` recria a lista de comandos a cada tecla | baixa | regressão |
| [#71](https://github.com/phduarte/tetris/issues/71) | Otimizar a pintura do tabuleiro em `Play` | baixa | manual |

## Fase 6 — Backlog antigo

Itens que estavam no backlog desde antes deste plano, revistos depois da fase 5.

| Issue | Item | Situação |
| --- | --- | --- |
| [#29](https://github.com/phduarte/tetris/issues/29) | Refactor `Board.cs` (aberta pelo Better Code Hub) | Fechada sem mudanças: coberta pelas fases 3 a 5 |
| [#30](https://github.com/phduarte/tetris/issues/30) | Refactor `Block.cs` (aberta pelo Better Code Hub) | Fechada sem mudanças: `Block` é um value object; fan-in alto é esperado |
| [#31](https://github.com/phduarte/tetris/issues/31) | Refactor `Stats.cs` | Contagem do tempo extraída para `PlayClock`; pontuação e níveis já tinham sido tratados em #48 e #68 |
| [#2](https://github.com/phduarte/tetris/issues/2) | HI-SCORE na tela de jogo | `Board.Record` acompanha a pontuação atual quando ela supera o recorde guardado; o `Play` mostra o recorde abaixo da pontuação |
| [#49](https://github.com/phduarte/tetris/issues/49) | README em inglês | `README.en.md` |
