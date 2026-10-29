using Gadz.Tetris.Domain.Models.Statistics;
using Gadz.Tetris.Web.Screens;
using Microsoft.AspNetCore.Components;

namespace Gadz.Tetris.Web.Device;

/// <summary>
/// O aparelho: a carcaça com o LCD e os botões. Exibe uma <see cref="ConsoleScreen"/> por vez e troca com
/// <see cref="Navigate{TScreen}"/>. Equivale ao <c>ConsoleFrame</c> do Desktop.
/// </summary>
public partial class ConsoleFrame
{
    /// <summary>
    /// Nome da seção da carcaça onde cada tela coloca os seus botões (ver <see cref="ConsoleButtons"/>).
    /// </summary>
    public const string BUTTONS_SECTION = "console-buttons";

    /// <summary>
    /// O tipo da tela exibida.
    /// </summary>
    private Type _screen = typeof(MenuScreen);

    /// <summary>
    /// Os parâmetros da tela exibida.
    /// </summary>
    private IDictionary<string, object?> _parameters = new Dictionary<string, object?>();

    /// <summary>
    /// A tela exibida, que recebe as teclas; registrada por ela mesma ao ser criada.
    /// </summary>
    private ConsoleScreen? _current;

    /// <summary>
    /// Onde as estatísticas das partidas são guardadas (no navegador, o <c>LocalStorageStatsRepository</c>).
    /// </summary>
    [Inject]
    public IStatsRepository Repository { get; set; } = default!;

    /// <summary>
    /// Troca a tela do LCD.
    /// </summary>
    /// <typeparam name="TScreen">A nova tela.</typeparam>
    /// <param name="parameters">Os parâmetros da nova tela, pelo nome da propriedade.</param>
    public void Navigate<TScreen>(IDictionary<string, object?>? parameters = null) where TScreen : ConsoleScreen
    {
        _current = null;
        _screen = typeof(TScreen);
        _parameters = parameters ?? new Dictionary<string, object?>();
        StateHasChanged();
    }

    /// <summary>
    /// Registra a tela exibida; chamado pela própria tela ao ser criada.
    /// </summary>
    /// <param name="screen">A tela.</param>
    internal void Attach(ConsoleScreen screen) => _current = screen;

    /// <summary>
    /// Repassa uma tecla (ou um botão clicado) para a tela exibida.
    /// </summary>
    /// <param name="key">A tecla, como o navegador a informa em <c>KeyboardEvent.key</c>.</param>
    /// <param name="control">Se o Ctrl estava apertado.</param>
    /// <param name="repeat">Se é uma repetição automática da tecla segurada.</param>
    /// <returns><c>true</c> se a tecla fez algo na tela.</returns>
    public bool Press(string key, bool control = false, bool repeat = false)
        => _current?.HandleKey(key, control, repeat) ?? false;
}
