using Gadz.Tetris.Application;
using Gadz.Tetris.Application.Commands;
using Gadz.Tetris.Domain.Models.Pieces;
using Gadz.Tetris.Texts;
using Gadz.Tetris.Web.Device;
using Gadz.Tetris.Web.Input;
using Microsoft.AspNetCore.Components;

namespace Gadz.Tetris.Web.Screens;

/// <summary>
/// A partida: tabuleiro em grid CSS à esquerda e painel lateral (pontos, linhas, próxima peça, nível, % de TETRIS,
/// tempo e recorde). A pausa e o fim de jogo acontecem dentro do próprio LCD. As teclas viram <see cref="Command"/>s;
/// nenhuma regra de jogo fica aqui — tudo é lido do <see cref="GameController"/>.
/// </summary>
public partial class PlayScreen
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
    /// Lado, em células, do quadro da próxima peça (cabe qualquer tetraminó).
    /// </summary>
    private const int NEXT_SIZE = 4;

    /// <summary>
    /// Posições do display da pontuação e do recorde.
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
    /// As cores da cortina, de baixo para cima (a mesma ordem da <c>Palette</c> do Desktop).
    /// </summary>
    private static readonly PieceColor[] _curtainColors =
        [PieceColor.Cyan, PieceColor.Purple, PieceColor.Orange, PieceColor.Blue, PieceColor.Yellow, PieceColor.Green, PieceColor.Red];

    /// <summary>
    /// Os comandos de teclado da partida.
    /// </summary>
    private CommandFactory _commandFactory = default!;

    /// <summary>
    /// Cancela a cortina e o tempo do GAME OVER quando a tela sai.
    /// </summary>
    private readonly CancellationTokenSource _leaving = new();

    /// <summary>
    /// Se o jogador perdeu: a cortina está subindo ou o GAME OVER está na tela.
    /// </summary>
    private bool _lost;

    /// <summary>
    /// Se a cortina terminou e o GAME OVER está na tela.
    /// </summary>
    private bool _gameOver;

    /// <summary>
    /// A partida, criada por <see cref="CreateGame"/> e ainda não iniciada (ela começa ao exibir a tela).
    /// </summary>
    [Parameter, EditorRequired]
    public GameController Controller { get; set; } = default!;

    /// <summary>
    /// Os botões da carcaça: durante a partida, pausar, sair, som e derrubar; no GAME OVER, som e continuar.
    /// </summary>
    private IReadOnlyList<ConsoleButton> Buttons => _lost
        ? [SoundButton, new(KeyMap.ENTER, Texts[nameof(Jogo.CapContinue)], Texts[nameof(Jogo.KeyAny)], Big: true)]
        : [
            new(KeyMap.ENTER, Texts[nameof(Jogo.CapPause)], Texts[nameof(Jogo.KeyEnter)], Latched: Paused),
            new(KeyMap.ESCAPE, Texts[nameof(Jogo.CapQuit)], Texts[nameof(Jogo.KeyEsc)]),
            SoundButton,
            new(KeyMap.SPACE, Texts[nameof(Jogo.CapDrop)], Texts[nameof(Jogo.KeySpace)], Big: true),
        ];

    /// <summary>
    /// O direcional: na partida, mover, girar e descer; no GAME OVER, qualquer seta continua.
    /// </summary>
    private DpadSetup Dpad => _lost
        ? new(true, true, Texts[nameof(Jogo.KeyAny)], Texts[nameof(Jogo.CapContinue)])
        : new(true, true, $"{Texts[nameof(Jogo.CapMove)]}\n{Texts[nameof(Jogo.CapDown)]}", Texts[nameof(Jogo.CapSlide)]);

    /// <summary>
    /// Se a partida está pausada; consulta o estado do jogo, sem flag paralela.
    /// </summary>
    private bool Paused => Controller.State.CanContinue;

    /// <summary>
    /// Cria uma partida no tamanho padrão, com o repositório e os sons do aparelho.
    /// </summary>
    /// <param name="frame">O aparelho.</param>
    /// <returns>A partida, ainda não iniciada.</returns>
    public static GameController CreateGame(ConsoleFrame frame)
        => GameController.Create(BOARD_WIDTH, BOARD_HEIGHT, frame.Repository, frame.Sound);

    /// <summary>
    /// Começa a partida e passa a ouvir os eventos dela.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _commandFactory = new CommandFactory(Controller);

        Controller.OnRefresh += RepaintAsync;
        Controller.OnClear += RepaintAsync;
        Controller.OnPause += RepaintAsync;
        Controller.OnContinue += RepaintAsync;
        Controller.OnFinish += ShowResultAsync;
        Controller.OnLose += StartCurtainAsync;

        Controller.Start();
    }

    /// <summary>
    /// Durante a partida, executa o comando da tecla (o primeiro que casar, na ordem do <see cref="CommandFactory"/>).
    /// Depois da cortina, qualquer tecla (um toque novo, não a repetição de uma tecla segurada) segue para o resultado.
    /// </summary>
    /// <param name="key">A tecla, como em <c>KeyboardEvent.key</c>.</param>
    /// <param name="control">Se o Ctrl estava apertado.</param>
    /// <param name="repeat">Se é uma repetição automática da tecla segurada (setas seguradas andam mais rápido).</param>
    /// <returns><c>true</c> se a tecla fez algo.</returns>
    public override bool HandleKey(string key, bool control, bool repeat)
    {
        if (_lost)
        {
            if (_gameOver && !repeat)
            {
                ShowResult();
            }

            return true;
        }

        if (!KeyMap.TryGetGameKey(key, out var gameKey))
        {
            return false;
        }

        var command = _commandFactory.GetAll(gameKey, control, repeat).FirstOrDefault();
        command?.Execute();

        // nem todo comando gera evento do jogo (ex.: o som), mas o painel mostra o resultado de todos
        StateHasChanged();
        return command is not null;
    }

    /// <summary>
    /// Pausa sozinha quando a janela do navegador perde o foco.
    /// </summary>
    public override void Deactivate()
    {
        if (Controller.Playing)
        {
            Controller.Pause();
        }
    }

    /// <summary>
    /// Para de ouvir a partida e cancela a cortina.
    /// </summary>
    public override void Dispose()
    {
        Controller.OnRefresh -= RepaintAsync;
        Controller.OnClear -= RepaintAsync;
        Controller.OnPause -= RepaintAsync;
        Controller.OnContinue -= RepaintAsync;
        Controller.OnFinish -= ShowResultAsync;
        Controller.OnLose -= StartCurtainAsync;

        _leaving.Cancel();
        _leaving.Dispose();
        base.Dispose();
    }

    /// <summary>
    /// A cor de uma célula do tabuleiro, lida do jogo.
    /// </summary>
    private PieceColor CellColor(int x, int y) => Controller.GetBlock(x, y).Color;

    /// <summary>
    /// A cor de uma célula do quadro da próxima peça.
    /// </summary>
    private PieceColor NextColor(int x, int y)
        => Controller.GetNextBlocks().FirstOrDefault(b => b.X == x && b.Y == y).Color;

    /// <summary>
    /// A classe CSS da cor de uma linha da cortina.
    /// </summary>
    private static string CurtainColor(int row)
        => _curtainColors[(BOARD_HEIGHT - 1 - row) % _curtainColors.Length].ToString().ToLowerInvariant();

    /// <summary>
    /// Quando a linha da cortina aparece (subindo de baixo para cima) e quando se apaga (descendo de cima para baixo),
    /// como atrasos das animações CSS.
    /// </summary>
    private static string CurtainStyle(int row)
    {
        var cover = CURTAIN_HOLD_MS + ((BOARD_HEIGHT - 1 - row) * CURTAIN_STEP_MS);
        var clear = CURTAIN_HOLD_MS + ((BOARD_HEIGHT + row) * CURTAIN_STEP_MS);
        return $"--cover: {cover}ms; --clear: {clear}ms";
    }

    /// <summary>
    /// Quando a cortina, toda erguida, começa a se apagar: a partir daí o fundo dela esconde o tabuleiro entre as células.
    /// </summary>
    private static int CurtainFillMs() => CURTAIN_HOLD_MS + (BOARD_HEIGHT * CURTAIN_STEP_MS);

    /// <summary>
    /// Redesenha pelo despachante do Blazor (os eventos do jogo chegam pelo relógio da Application).
    /// </summary>
    private void RepaintAsync() => _ = InvokeAsync(StateHasChanged);

    /// <summary>
    /// Ao perder, começa a cortina.
    /// </summary>
    private void StartCurtainAsync() => _ = InvokeAsync(RunCurtainAsync);

    /// <summary>
    /// A cortina sobe (as animações ficam no CSS; aqui só os tempos), a música de fim toca, o GAME OVER aparece
    /// e, se ninguém apertar nada, segue sozinho para o resultado.
    /// </summary>
    /// <returns>A tarefa da cortina, cancelada se a tela sair antes.</returns>
    private async Task RunCurtainAsync()
    {
        _lost = true;
        StateHasChanged();

        try
        {
            await Task.Delay(CURTAIN_HOLD_MS, _leaving.Token);
            Frame.Sound.End();

            await Task.Delay(2 * BOARD_HEIGHT * CURTAIN_STEP_MS, _leaving.Token);
            _gameOver = true;
            StateHasChanged();

            await Task.Delay(GAME_OVER_TIMEOUT_MS, _leaving.Token);
            ShowResult();
        }
        catch (OperationCanceledException)
        {
            // a tela saiu antes (o jogador apertou uma tecla no GAME OVER)
        }
    }

    /// <summary>
    /// Ao encerrar com Esc, vai direto para o resultado.
    /// </summary>
    private void ShowResultAsync() => _ = InvokeAsync(ShowResult);

    /// <summary>
    /// Mostra o resultado da partida.
    /// </summary>
    private void ShowResult() => Frame.Navigate<ResultScreen>(new Dictionary<string, object?>
    {
        [nameof(ResultScreen.Stats)] = Controller.GetStats(),
        [nameof(ResultScreen.IsNewRecord)] = Controller.IsNewRecord,
    });
}
