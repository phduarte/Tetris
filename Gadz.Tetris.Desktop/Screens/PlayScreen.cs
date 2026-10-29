using System.Drawing.Drawing2D;
using Gadz.Tetris.Application;
using Gadz.Tetris.Application.Commands;
using Gadz.Tetris.Desktop.Device;
using Gadz.Tetris.Desktop.Extensions;
using Gadz.Tetris.Domain.Models.Pieces;
using Texto = Gadz.Tetris.Texts.Jogo;

namespace Gadz.Tetris.Desktop.Screens;

/// <summary>
/// A partida: tabuleiro à esquerda e painel lateral (pontos, linhas, próxima peça, nível, % de TETRIS e tempo).
/// A pausa e o fim de jogo acontecem dentro do próprio LCD. As teclas viram <see cref="Command"/>s; nenhuma regra de jogo fica aqui.
/// </summary>
internal sealed class PlayScreen : ConsoleScreen
{
    /// <summary>
    /// Largura do tabuleiro, em células.
    /// </summary>
    private const int BOARD_WIDTH = 10;

    /// <summary>
    /// Altura do tabuleiro, em células.
    /// </summary>
    private const int BOARD_HEIGHT = 20;

    /// <summary>
    /// Lado das células do tabuleiro.
    /// </summary>
    private const float CELL = 16F;

    /// <summary>
    /// Espaço entre as células.
    /// </summary>
    private const float CELL_GAP = 2F;

    /// <summary>
    /// Espaço entre o tabuleiro e a linha que o separa do painel.
    /// </summary>
    private const float BOARD_PADDING = 6F;

    /// <summary>
    /// Espessura da linha entre o tabuleiro e o painel.
    /// </summary>
    private const float SEPARATOR = 2F;

    /// <summary>
    /// Espaço entre a linha e o painel.
    /// </summary>
    private const float SIDE_GAP = 12F;

    /// <summary>
    /// Espaço entre os itens do painel.
    /// </summary>
    private const float STAT_GAP = 11F;

    /// <summary>
    /// Espaço entre o rótulo e o valor de um item do painel.
    /// </summary>
    private const float LABEL_GAP = 5F;

    /// <summary>
    /// Altura dos dígitos da pontuação.
    /// </summary>
    private const float SCORE_HEIGHT = 24F;

    /// <summary>
    /// Altura dos dígitos de linhas, nível e % de TETRIS.
    /// </summary>
    private const float STAT_HEIGHT = 20F;

    /// <summary>
    /// Altura dos dígitos do tempo.
    /// </summary>
    private const float TIME_HEIGHT = 17F;

    /// <summary>
    /// Posições do display da pontuação.
    /// </summary>
    private const int SCORE_DIGITS = 6;

    /// <summary>
    /// Posições do display de linhas.
    /// </summary>
    private const int LINES_DIGITS = 4;

    /// <summary>
    /// Posições do display do nível.
    /// </summary>
    private const int LEVEL_DIGITS = 2;

    /// <summary>
    /// Posições do display da % de TETRIS.
    /// </summary>
    private const int RATE_DIGITS = 3;

    /// <summary>
    /// Lado, em células, do quadro da próxima peça (cabe qualquer tetraminó).
    /// </summary>
    private const int NEXT_SIZE = 4;

    /// <summary>
    /// Lado das células do quadro da próxima peça.
    /// </summary>
    private const float NEXT_CELL = 13F;

    /// <summary>
    /// Tamanho dos indicadores de som e de pausa.
    /// </summary>
    private const float ICON_SIZE = 15F;

    /// <summary>
    /// Espaço entre os indicadores.
    /// </summary>
    private const float ICON_GAP = 10F;

    /// <summary>
    /// Opacidade de um indicador apagado.
    /// </summary>
    private const float ICON_OFF_OPACITY = 0.1F;

    /// <summary>
    /// Tamanho do título da pausa.
    /// </summary>
    private const float PAUSE_SIZE = 20F;

    /// <summary>
    /// Tamanho do "GAME OVER".
    /// </summary>
    private const float GAME_OVER_SIZE = 30F;

    /// <summary>
    /// Tamanho das instruções sob os títulos.
    /// </summary>
    private const float HINT_SIZE = 9F;

    /// <summary>
    /// Espaço entre o título e a instrução.
    /// </summary>
    private const float OVERLAY_GAP = 10F;

    /// <summary>
    /// Espera, em milissegundos, entre perder e a cortina começar.
    /// </summary>
    private const int CURTAIN_HOLD_MS = 700;

    /// <summary>
    /// Intervalo, em milissegundos, entre as linhas da cortina.
    /// </summary>
    private const int CURTAIN_STEP_MS = 45;

    /// <summary>
    /// Por quanto tempo, no máximo, o GAME OVER fica na tela antes de seguir sozinho para o resultado.
    /// </summary>
    private const int GAME_OVER_TIMEOUT_MS = 10_000;

    /// <summary>
    /// Cor do topo do "GAME OVER" no estilo TETRIS.
    /// </summary>
    private static readonly Color _gameOverTop = Palette.Of(PieceColor.Yellow);

    /// <summary>
    /// Cor da base do "GAME OVER" no estilo TETRIS.
    /// </summary>
    private static readonly Color _gameOverBottom = Palette.Of(PieceColor.Red);

    /// <summary>
    /// A partida.
    /// </summary>
    private readonly GameController _controller;

    /// <summary>
    /// Os comandos de teclado da partida.
    /// </summary>
    private readonly CommandFactory _commandFactory;

    /// <summary>
    /// A thread de UI: o <c>Board</c> avisa as mudanças a partir da thread do loop, e o redesenho tem que voltar para cá.
    /// </summary>
    private readonly TaskScheduler _threadPrincipal;

    /// <summary>
    /// A cortina do fim de jogo; existe só depois de perder.
    /// </summary>
    private GameOverCurtain? _curtain;

    /// <summary>
    /// Leva ao resultado se o jogador não apertar nenhuma tecla no GAME OVER. Timer de WinForms: dispara na thread de UI.
    /// </summary>
    private readonly System.Windows.Forms.Timer _gameOverTimeout = new() { Interval = GAME_OVER_TIMEOUT_MS };

    /// <summary>
    /// Cria a tela para uma partida ainda não iniciada (ela começa em <see cref="Enter"/>).
    /// </summary>
    /// <param name="frame">O aparelho.</param>
    /// <param name="controller">A partida, criada por <see cref="CreateGame"/>.</param>
    public PlayScreen(ConsoleFrame frame, GameController controller) : base(frame)
    {
        _controller = controller;
        _commandFactory = new CommandFactory(controller);
        _threadPrincipal = TaskScheduler.FromCurrentSynchronizationContext();
        _gameOverTimeout.Tick += (_, _) => ShowResult();
    }

    /// <inheritdoc/>
    public override ConsoleLayout Layout => (_curtain, Paused) switch
    {
        (not null, _) => new ConsoleLayout(
            new DpadSetup(true, true, Texto.KeyAny, Texto.CapContinue),
            [SoundButton],
            new ConsoleButton(Keys.Enter, Texto.CapContinue, Texto.KeyAny)),
        (null, var paused) => new ConsoleLayout(
            new DpadSetup(true, true, $"{Texto.CapMove}\n{Texto.CapDown}", Texto.CapSlide),
            [new ConsoleButton(Keys.Enter, Texto.CapPause, Texto.KeyEnter, Pulse: paused), new ConsoleButton(Keys.Escape, Texto.CapQuit, Texto.KeyEsc), SoundButton],
            new ConsoleButton(Keys.Space, Texto.CapDrop, Texto.KeySpace)),
    };

    /// <inheritdoc/>
    public override bool IsAnimating => _curtain is { Finished: false };

    /// <summary>
    /// Se a partida está pausada; consulta o estado do jogo, sem flag paralela.
    /// </summary>
    private bool Paused => _controller.State.CanContinue;

    /// <summary>
    /// Cria uma partida no tamanho padrão.
    /// </summary>
    /// <param name="frame">O aparelho, que fornece o repositório e os sons.</param>
    /// <returns>A partida, ainda não iniciada.</returns>
    public static GameController CreateGame(ConsoleFrame frame) => GameController.Create(BOARD_WIDTH, BOARD_HEIGHT, frame.Repository, frame.Sound);

    /// <summary>
    /// Começa a partida e passa a ouvir os eventos dela.
    /// </summary>
    public override void Enter()
    {
        _controller.OnRefresh += RepaintAsync;
        _controller.OnClear += RepaintAsync;
        _controller.OnPause += RepaintAsync;
        _controller.OnContinue += RepaintAsync;
        _controller.OnFinish += ShowResultAsync;
        _controller.OnLose += StartCurtainAsync;
        _controller.OnDrop += ShakeDown;

        _controller.Start();
    }

    /// <summary>
    /// Para de ouvir a partida e a cortina.
    /// </summary>
    public override void Leave()
    {
        _controller.OnRefresh -= RepaintAsync;
        _controller.OnClear -= RepaintAsync;
        _controller.OnPause -= RepaintAsync;
        _controller.OnContinue -= RepaintAsync;
        _controller.OnFinish -= ShowResultAsync;
        _controller.OnLose -= StartCurtainAsync;
        _controller.OnDrop -= ShakeDown;

        _curtain?.Dispose();
        _gameOverTimeout.Dispose();
    }

    /// <summary>
    /// Pausa sozinha quando a janela perde o foco.
    /// </summary>
    public override void Deactivate()
    {
        if (_controller.Playing)
        {
            _controller.Pause();
        }
    }

    /// <summary>
    /// O ✕ durante a partida encerra e salva, como o Esc; depois do fim de jogo, fecha o aparelho.
    /// </summary>
    public override void Close()
    {
        if (_curtain is null)
        {
            _controller.Exit();
            return;
        }

        base.Close();
    }

    /// <summary>
    /// Durante a partida, executa o comando da tecla (o primeiro que casar, na ordem do <see cref="CommandFactory"/>).
    /// Depois da cortina, qualquer tecla (um toque novo, não a repetição de uma tecla que já estava segurada) segue para o resultado.
    /// </summary>
    /// <param name="key">A tecla.</param>
    /// <param name="control">Se o Ctrl estava apertado.</param>
    /// <param name="repeat">Se é uma repetição automática da tecla segurada (setas seguradas andam mais rápido).</param>
    /// <returns><c>true</c> se a tecla fez algo.</returns>
    public override bool HandleKey(Keys key, bool control, bool repeat)
    {
        if (_curtain is not null)
        {
            if (_curtain.Finished && !repeat)
            {
                ShowResult();
            }

            return true;
        }

        var command = _commandFactory.GetAll((Key)key, control, repeat).FirstOrDefault();
        command?.Execute();
        return command is not null;
    }

    /// <inheritdoc/>
    public override void Paint(LcdCanvas lcd)
    {
        var left = LcdCanvas.PADDING;
        var top = LcdCanvas.PADDING;
        var boardWidth = LcdCanvas.GridSize(BOARD_WIDTH, CELL, CELL_GAP);
        var board = new RectangleF(left, top, boardWidth + BOARD_PADDING, LcdCanvas.GridSize(BOARD_HEIGHT, CELL, CELL_GAP));

        lcd.Grid(left, top, BOARD_WIDTH, BOARD_HEIGHT, CELL, CELL_GAP, CellColor);
        lcd.Fill(new RectangleF(board.Right, top, SEPARATOR, board.Height));

        if (_curtain is { Finished: true })
        {
            PaintGameOver(lcd, board);
        }
        else if (_curtain is null && Paused)
        {
            PaintPause(lcd, board);
        }

        PaintSide(lcd, board.Right + SEPARATOR + SIDE_GAP, top);
    }

    /// <summary>
    /// A cor de uma célula do tabuleiro: a da cortina, se ela a cobre, ou a do jogo.
    /// </summary>
    private PieceColor CellColor(int x, int y) => _curtain?.ColorAt(y) ?? _controller.GetBlock(x, y).Color;

    /// <summary>
    /// Desenha o painel lateral.
    /// </summary>
    private void PaintSide(LcdCanvas lcd, float left, float top)
    {
        var right = LcdCanvas.WIDTH - LcdCanvas.PADDING;
        var y = top;

        y = PaintStat(lcd, Texto.Pontuacao, left, y, SCORE_HEIGHT, (x, valueTop) => lcd.Number(_controller.Score, SCORE_DIGITS, x, valueTop, SCORE_HEIGHT));
        y = PaintStat(lcd, Texto.Linhas, left, y, STAT_HEIGHT, (x, valueTop) => lcd.Number(_controller.Lines, LINES_DIGITS, x, valueTop, STAT_HEIGHT));

        var nextBlocks = _controller.GetNextBlocks().ToList();
        y = PaintStat(lcd, Texto.Proximo, left, y, LcdCanvas.GridSize(NEXT_SIZE, NEXT_CELL, CELL_GAP), (x, valueTop) =>
            lcd.Grid(x, valueTop, NEXT_SIZE, NEXT_SIZE, NEXT_CELL, CELL_GAP,
                (column, row) => nextBlocks.FirstOrDefault(b => b.X == column && b.Y == row).Color));

        var rateLeft = right - Math.Max(lcd.MeasurePercent(RATE_DIGITS, STAT_HEIGHT), lcd.MeasureLabel(Texto.TaxaTetris));
        PaintStat(lcd, Texto.Nivel, left, y, STAT_HEIGHT, (x, valueTop) => lcd.Number(_controller.Level, LEVEL_DIGITS, x, valueTop, STAT_HEIGHT));
        y = PaintStat(lcd, Texto.TaxaTetris, rateLeft, y, STAT_HEIGHT, (x, valueTop) => lcd.Percent(_controller.TetrisRate, RATE_DIGITS, x, valueTop, STAT_HEIGHT));

        y = PaintStat(lcd, Texto.Tempo, left, y, TIME_HEIGHT, (x, valueTop) => lcd.Time(_controller.Duration, x, valueTop, TIME_HEIGHT));
        y = PaintStat(lcd, Texto.Recorde, left, y, TIME_HEIGHT, (x, valueTop) => lcd.Number(_controller.GetMaxScore(), SCORE_DIGITS, x, valueTop, TIME_HEIGHT));

        PaintIcons(lcd, left, y);
    }

    /// <summary>
    /// Desenha um item do painel: o rótulo e, logo abaixo, o valor.
    /// </summary>
    /// <returns>O topo do próximo item.</returns>
    private static float PaintStat(LcdCanvas lcd, string label, float left, float top, float valueHeight, Action<float, float> paintValue)
    {
        lcd.Label(label, left, top);
        var valueTop = top + LcdCanvas.LABEL_SIZE + LABEL_GAP;
        paintValue(left, valueTop);
        return valueTop + valueHeight + STAT_GAP;
    }

    /// <summary>
    /// Desenha os indicadores de som (♪) e de pausa (❚❚), acesos ou apagados como num aparelho.
    /// </summary>
    private void PaintIcons(LcdCanvas lcd, float left, float top)
    {
        using var on = lcd.InkBrush(1);
        using var off = lcd.InkBrush(ICON_OFF_OPACITY);

        var note = "♪";
        var font = Fonts.Bold(ICON_SIZE);
        lcd.Text(note, font, left, top, brush: _controller.IsMuted ? off : on);

        // as barras da pausa são desenhadas, e não escritas: o "❚" de cada fonte tem um tamanho
        var barsLeft = left + lcd.MeasureText(note, font) + ICON_GAP;
        var barWidth = ICON_SIZE / 5;
        var barTop = top + (ICON_SIZE * 0.2F);
        var barHeight = ICON_SIZE * 0.8F;
        var brush = Paused && _curtain is null ? on : off;
        lcd.Graphics.FillRectangle(brush, barsLeft, barTop, barWidth, barHeight);
        lcd.Graphics.FillRectangle(brush, barsLeft + (2 * barWidth), barTop, barWidth, barHeight);
    }

    /// <summary>
    /// Apaga o tabuleiro (sem espiar a jogada) e mostra PAUSA e como continuar.
    /// </summary>
    private static void PaintPause(LcdCanvas lcd, RectangleF board)
    {
        lcd.Clear(board);
        var center = board.X + (board.Width / 2);
        var top = board.Y + ((board.Height - PAUSE_SIZE - OVERLAY_GAP - HINT_SIZE) / 2);

        lcd.TextCentered(Texto.Pausado.ToUpperInvariant(), Fonts.Pixel(PAUSE_SIZE), center, top);
        if (lcd.BlinkOn)
        {
            lcd.TextCentered(Texto.PressioneEnterParaContinuar.ToUpperInvariant(), Fonts.Pixel(HINT_SIZE), center, top + PAUSE_SIZE + OVERLAY_GAP);
        }
    }

    /// <summary>
    /// Mostra o "GAME OVER" em duas linhas (com degradê amarelo → vermelho no estilo TETRIS) e o convite para continuar.
    /// </summary>
    private static void PaintGameOver(LcdCanvas lcd, RectangleF board)
    {
        lcd.Clear(board);
        var center = board.X + (board.Width / 2);
        var lines = Texto.GameOver.ToUpperInvariant().Split(' ', 2);
        var lineHeight = GAME_OVER_SIZE * 1.05F;
        var textHeight = lines.Length * lineHeight;
        var top = board.Y + ((board.Height - textHeight - OVERLAY_GAP - HINT_SIZE) / 2);

        using var gradient = new LinearGradientBrush(new RectangleF(board.X, top, board.Width, textHeight), _gameOverTop, _gameOverBottom, 90F);
        using var ink = lcd.InkBrush(1);
        Brush brush = lcd.Theme.Colored ? gradient : ink;

        for (var i = 0; i < lines.Length; i++)
        {
            lcd.TextCentered(lines[i], Fonts.Pixel(GAME_OVER_SIZE), center, top + (i * lineHeight), brush: brush);
        }

        if (lcd.BlinkOn)
        {
            lcd.TextCentered(Texto.PressioneParaContinuar.ToUpperInvariant(), Fonts.Pixel(HINT_SIZE), center, top + textHeight + OVERLAY_GAP);
        }
    }

    /// <summary>
    /// Redesenha na thread de UI (os eventos do jogo chegam pela thread do loop).
    /// </summary>
    private void RepaintAsync() => _ = RunOnUiAsync(Frame.Invalidate);

    /// <summary>
    /// Ao perder, começa a cortina na thread de UI.
    /// </summary>
    private void StartCurtainAsync() => _ = RunOnUiAsync(() =>
    {
        _curtain = new GameOverCurtain(BOARD_HEIGHT, CURTAIN_HOLD_MS, CURTAIN_STEP_MS, Frame.Sound.End, OnCurtainStep);
        Frame.Invalidate();
    });

    /// <summary>
    /// A cada passo da cortina, redesenha; quando ela termina e o GAME OVER aparece, começa a contar o tempo máximo dele na tela.
    /// </summary>
    private void OnCurtainStep()
    {
        Frame.Invalidate();

        if (_curtain is { Finished: true })
        {
            _gameOverTimeout.Start();
        }
    }

    /// <summary>
    /// Ao encerrar com Esc, vai direto para o resultado.
    /// </summary>
    private void ShowResultAsync() => _ = RunOnUiAsync(ShowResult);

    /// <summary>
    /// Mostra o resultado da partida.
    /// </summary>
    private void ShowResult()
    {
        // pela tecla ou pelo tempo esgotado, o que vier primeiro: o timer não pode disparar de novo
        _gameOverTimeout.Stop();
        Frame.Navigate(new ResultScreen(Frame, _controller.GetStats(), _controller.IsNewRecord));
    }

    /// <summary>
    /// Sacode o aparelho quando a peça é derrubada.
    /// </summary>
    private void ShakeDown() => Frame.ShakeDown(2);

    /// <summary>
    /// Agenda a ação na thread de UI, pelo <see cref="TaskScheduler"/> capturado ao criar a tela.
    /// Quem chama são os eventos do jogo, que não esperam pela tela: a tarefa é descartada.
    /// </summary>
    /// <returns>A tarefa que executa a ação.</returns>
    private Task RunOnUiAsync(Action action)
        => Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.None, _threadPrincipal);
}
