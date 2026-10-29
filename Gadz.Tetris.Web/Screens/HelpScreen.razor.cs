using Gadz.Tetris.Application;
using Gadz.Tetris.Texts;
using Gadz.Tetris.Web.Device;
using Gadz.Tetris.Web.Input;

namespace Gadz.Tetris.Web.Screens;

/// <summary>
/// Ajuda: as teclas do jogo (cada linha acende quando a tecla dela é apertada, para testar), a tabela de pontos,
/// lida de <see cref="GameController.ScoreTable"/> para não repetir os números, e a regra do nível.
/// </summary>
public partial class HelpScreen
{
    /// <summary>
    /// Por quanto tempo, em milissegundos, a linha da tecla apertada fica acesa.
    /// </summary>
    private const int HIGHLIGHT_MS = 450;

    /// <summary>
    /// As linhas da tabela de teclas.
    /// </summary>
    private IReadOnlyList<HelpRow> _rows = [];

    /// <summary>
    /// As linhas acesas no momento.
    /// </summary>
    private readonly HashSet<int> _highlighted = [];

    /// <summary>
    /// Cancela as linhas acesas quando a tela sai.
    /// </summary>
    private readonly CancellationTokenSource _leaving = new();

    /// <inheritdoc/>
    protected override IReadOnlyDictionary<string, Action> KeyBindings => new Dictionary<string, Action>
    {
        [KeyMap.ESCAPE] = () => Frame.Navigate<MenuScreen>(),
        [KeyMap.SHIFT] = ToggleSound,
    };

    /// <summary>
    /// O direcional: qualquer seta acende a sua linha, para testar.
    /// </summary>
    private DpadSetup Dpad => new(true, true, Texts[nameof(Jogo.CapTest)], Texts[nameof(Jogo.KeyAny)]);

    /// <summary>
    /// Os botões da carcaça: som e voltar.
    /// </summary>
    private IReadOnlyList<ConsoleButton> Buttons =>
        [SoundButton, new(KeyMap.ESCAPE, Texts[nameof(Jogo.CapBack)], Texts[nameof(Jogo.KeyEsc)], Big: true)];

    /// <summary>
    /// Monta a tabela de teclas com os textos na cultura do navegador.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _rows =
        [
            new(["◀", "▶"], Texts[nameof(Jogo.HelpMove)], [KeyMap.LEFT, KeyMap.RIGHT], Control: false),
            new([Texts[nameof(Jogo.KeyCtrl)], "◀", "▶"], Texts[nameof(Jogo.HelpSlide)], [KeyMap.LEFT, KeyMap.RIGHT], Control: true),
            new(["▼"], Texts[nameof(Jogo.HelpSoftDrop)], [KeyMap.DOWN]),
            new(["▲"], Texts[nameof(Jogo.HelpRotate)], [KeyMap.UP]),
            new([Texts[nameof(Jogo.KeySpace)]], Texts[nameof(Jogo.HelpHardDrop)], [KeyMap.SPACE]),
            new([Texts[nameof(Jogo.KeyEnter)]], Texts[nameof(Jogo.HelpPause)], [KeyMap.ENTER]),
            new([Texts[nameof(Jogo.KeyShift)]], Texts[nameof(Jogo.HelpSound)], [KeyMap.SHIFT]),
            new([Texts[nameof(Jogo.KeyEsc)]], Texts[nameof(Jogo.HelpQuit)], [KeyMap.ESCAPE]),
        ];
    }

    /// <summary>
    /// Acende as linhas da tecla apertada e depois executa a navegação dela, se houver.
    /// </summary>
    /// <param name="key">A tecla, como em <c>KeyboardEvent.key</c>.</param>
    /// <param name="control">Se o Ctrl estava apertado.</param>
    /// <param name="repeat">Se é uma repetição automática da tecla segurada.</param>
    /// <returns><c>true</c> se a tecla acendeu alguma linha ou fez algo na tela.</returns>
    public override bool HandleKey(string key, bool control, bool repeat)
    {
        var matched = false;

        for (var i = 0; i < _rows.Count; i++)
        {
            if (_rows[i].Matches(key, control))
            {
                matched = true;
                var row = i;
                _ = InvokeAsync(() => HighlightAsync(row));
            }
        }

        return base.HandleKey(key, control, repeat) || matched;
    }

    /// <summary>
    /// Para de acender as linhas.
    /// </summary>
    public override void Dispose()
    {
        _leaving.Cancel();
        _leaving.Dispose();
        base.Dispose();
    }

    /// <summary>
    /// Acende a linha por um instante.
    /// </summary>
    /// <param name="row">A linha.</param>
    /// <returns>A tarefa, cancelada se a tela sair antes.</returns>
    private async Task HighlightAsync(int row)
    {
        _highlighted.Add(row);
        StateHasChanged();

        try
        {
            await Task.Delay(HIGHLIGHT_MS, _leaving.Token);
            _highlighted.Remove(row);
            StateHasChanged();
        }
        catch (OperationCanceledException)
        {
            // a tela saiu antes
        }
    }

    /// <summary>
    /// O texto de uma linha da tabela de pontos ("1 linha", "2 linhas", "4 linhas · tetris!").
    /// </summary>
    private string LinesText(int lines)
    {
        var format = lines == 1 ? nameof(Jogo.OneLine) : lines == GameController.ScoreTable.Count ? nameof(Jogo.TetrisLines) : nameof(Jogo.ManyLines);
        return string.Format(Texts[format], lines);
    }

    /// <summary>
    /// Uma linha da tabela de teclas.
    /// </summary>
    /// <param name="Caps">As teclas desenhadas.</param>
    /// <param name="Text">O que elas fazem.</param>
    /// <param name="Keys">As teclas do navegador que acendem a linha.</param>
    /// <param name="Control">Se a linha exige (true) ou recusa (false) o Ctrl; nulo para tanto faz.</param>
    private sealed record HelpRow(string[] Caps, string Text, string[] Keys, bool? Control = null)
    {
        /// <summary>
        /// Se a tecla apertada acende esta linha.
        /// </summary>
        public bool Matches(string key, bool control) => Keys.Contains(key) && (Control is null || Control == control);
    }
}
