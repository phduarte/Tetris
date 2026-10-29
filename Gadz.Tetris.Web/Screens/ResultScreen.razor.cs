using Gadz.Tetris.Application;
using Gadz.Tetris.Texts;
using Gadz.Tetris.Web.Device;
using Gadz.Tetris.Web.Input;
using Microsoft.AspNetCore.Components;

namespace Gadz.Tetris.Web.Screens;

/// <summary>
/// Resultado da partida: pontuação, selo de novo recorde e as demais estatísticas.
/// Enter ou Esc voltam ao menu; sem nenhuma tecla, volta sozinho depois de um tempo.
/// </summary>
public partial class ResultScreen
{
    /// <summary>
    /// Posições do display da pontuação.
    /// </summary>
    private const int SCORE_DIGITS = 6;

    /// <summary>
    /// Posições do display do nível.
    /// </summary>
    private const int LEVEL_DIGITS = 2;

    /// <summary>
    /// Posições dos displays de linhas e peças.
    /// </summary>
    private const int COUNT_DIGITS = 4;

    /// <summary>
    /// Posições do display da % de TETRIS.
    /// </summary>
    private const int RATE_DIGITS = 3;

    /// <summary>
    /// Tempo, em milissegundos, até voltar sozinho ao menu.
    /// </summary>
    private const int TIMEOUT_MS = 30_000;

    /// <summary>
    /// Cancela a volta automática ao menu quando a tela sai antes.
    /// </summary>
    private readonly CancellationTokenSource _leaving = new();

    /// <summary>
    /// As estatísticas da partida.
    /// </summary>
    [Parameter, EditorRequired]
    public StatsSnapshot Stats { get; set; }

    /// <summary>
    /// Se a partida superou o recorde guardado.
    /// </summary>
    [Parameter]
    public bool IsNewRecord { get; set; }

    /// <inheritdoc/>
    protected override IReadOnlyDictionary<string, Action> KeyBindings => new Dictionary<string, Action>
    {
        [KeyMap.ENTER] = BackToMenu,
        [KeyMap.ESCAPE] = BackToMenu,
        [KeyMap.SHIFT] = ToggleSound,
    };

    /// <summary>
    /// O direcional, sem uso nesta tela.
    /// </summary>
    private static DpadSetup Dpad { get; } = new(false, false, string.Empty);

    /// <summary>
    /// Os botões da carcaça: som e concluir.
    /// </summary>
    private IReadOnlyList<ConsoleButton> Buttons =>
        [SoundButton, new(KeyMap.ENTER, Texts[nameof(Jogo.CapDone)], Texts[nameof(Jogo.KeyEnter)], Big: true)];

    /// <summary>
    /// Começa a contar o tempo para voltar ao menu sozinho.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _ = InvokeAsync(TimeoutAsync);
    }

    /// <summary>
    /// Para a contagem do tempo.
    /// </summary>
    public override void Dispose()
    {
        _leaving.Cancel();
        _leaving.Dispose();
        base.Dispose();
    }

    /// <summary>
    /// Volta ao menu se ninguém apertar nada até o tempo esgotar.
    /// </summary>
    /// <returns>A tarefa da espera, cancelada se a tela sair antes.</returns>
    private async Task TimeoutAsync()
    {
        try
        {
            await Task.Delay(TIMEOUT_MS, _leaving.Token);
            BackToMenu();
        }
        catch (OperationCanceledException)
        {
            // o jogador voltou ao menu antes
        }
    }

    /// <summary>
    /// Volta ao menu.
    /// </summary>
    private void BackToMenu() => Frame.Navigate<MenuScreen>();
}
