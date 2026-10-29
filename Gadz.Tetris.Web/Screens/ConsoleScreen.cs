using Gadz.Tetris.Texts;
using Gadz.Tetris.Web.Device;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Gadz.Tetris.Web.Screens;

/// <summary>
/// Uma tela do jogo. O aparelho (<see cref="ConsoleFrame"/>) é sempre o mesmo; a tela decide o que aparece no LCD,
/// quais botões a carcaça mostra e o que cada tecla faz. Equivale à <c>ConsoleScreen</c> do Desktop.
/// </summary>
public abstract class ConsoleScreen : ComponentBase, IDisposable
{
    /// <summary>
    /// O aparelho onde a tela é exibida.
    /// </summary>
    [CascadingParameter]
    public ConsoleFrame Frame { get; set; } = default!;

    /// <summary>
    /// Os textos de interface, na cultura do navegador.
    /// </summary>
    [Inject]
    protected IStringLocalizer<Jogo> Texts { get; set; } = default!;

    /// <summary>
    /// O que cada tecla (<c>KeyboardEvent.key</c>) faz nesta tela. Uma tabela, e não um <c>switch</c> de teclas,
    /// para cada tela só declarar as suas.
    /// </summary>
    protected virtual IReadOnlyDictionary<string, Action> KeyBindings { get; } = new Dictionary<string, Action>();

    /// <summary>
    /// Trata uma tecla apertada (ou um botão clicado).
    /// </summary>
    /// <param name="key">A tecla, como o navegador a informa em <c>KeyboardEvent.key</c>.</param>
    /// <param name="control">Se o Ctrl estava apertado.</param>
    /// <param name="repeat">Se é uma repetição automática da tecla segurada; fora da partida, vale como um novo toque.</param>
    /// <returns><c>true</c> se a tecla faz algo nesta tela.</returns>
    public virtual bool HandleKey(string key, bool control, bool repeat)
    {
        if (!KeyBindings.TryGetValue(key, out var action))
        {
            return false;
        }

        action();
        StateHasChanged();
        return true;
    }

    /// <summary>
    /// Chamado quando a janela do navegador perde o foco.
    /// </summary>
    public virtual void Deactivate()
    {
    }

    /// <summary>
    /// Avisa o aparelho que esta é a tela exibida, para ele repassar as teclas.
    /// </summary>
    protected override void OnInitialized() => Frame.Attach(this);

    /// <summary>
    /// Chamado quando a tela deixa de ser exibida (o aparelho trocou de tela).
    /// </summary>
    public virtual void Dispose() => GC.SuppressFinalize(this);
}
