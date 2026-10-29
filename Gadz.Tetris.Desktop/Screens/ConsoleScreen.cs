using Gadz.Tetris.Desktop.Device;
using Texto = Gadz.Tetris.Desktop.Textos.Jogo;

namespace Gadz.Tetris.Desktop.Screens;

/// <summary>
/// Uma tela do jogo. O aparelho (<see cref="ConsoleFrame"/>) é sempre o mesmo; a tela decide o que aparece no LCD,
/// quais botões a carcaça mostra e o que cada tecla faz.
/// </summary>
/// <param name="frame">O aparelho onde a tela é exibida.</param>
internal abstract class ConsoleScreen(ConsoleFrame frame)
{
    /// <summary>
    /// O aparelho onde a tela é exibida.
    /// </summary>
    protected ConsoleFrame Frame { get; } = frame;

    /// <summary>
    /// Os botões que a carcaça mostra nesta tela.
    /// </summary>
    public abstract ConsoleLayout Layout { get; }

    /// <summary>
    /// Se a tela tem uma animação em andamento e precisa ser redesenhada a cada quadro.
    /// </summary>
    public virtual bool IsAnimating => false;

    /// <summary>
    /// O que cada tecla faz nesta tela. Uma tabela, e não um <c>switch</c> de teclas, para cada tela só declarar as suas.
    /// </summary>
    protected virtual IReadOnlyDictionary<Keys, Action> KeyBindings { get; } = new Dictionary<Keys, Action>();

    /// <summary>
    /// O botão SOM, presente em todas as telas; com o som desligado, ele fica afundado.
    /// </summary>
    protected ConsoleButton SoundButton { get; } = new(Keys.ShiftKey, Texto.Sound, Texto.KeyShift, Latched: () => frame.Sound.Mute);

    /// <summary>
    /// Desenha o conteúdo do LCD.
    /// </summary>
    /// <param name="lcd">O LCD.</param>
    public abstract void Paint(LcdCanvas lcd);

    /// <summary>
    /// Trata uma tecla apertada (ou um botão clicado).
    /// </summary>
    /// <param name="key">A tecla.</param>
    /// <param name="control">Se o Ctrl estava apertado.</param>
    /// <param name="repeat">Se é uma repetição automática da tecla segurada; fora da partida, vale como um novo toque.</param>
    /// <returns><c>true</c> se a tecla faz algo nesta tela.</returns>
    public virtual bool HandleKey(Keys key, bool control, bool repeat)
    {
        if (!KeyBindings.TryGetValue(key, out var action))
        {
            return false;
        }

        action();
        return true;
    }

    /// <summary>
    /// Chamado quando a tela passa a ser exibida.
    /// </summary>
    public virtual void Enter()
    {
    }

    /// <summary>
    /// Chamado quando a tela deixa de ser exibida.
    /// </summary>
    public virtual void Leave()
    {
    }

    /// <summary>
    /// Chamado quando a janela perde o foco.
    /// </summary>
    public virtual void Deactivate()
    {
    }

    /// <summary>
    /// O botão de fechar (✕) foi clicado. Por padrão, fecha o jogo.
    /// </summary>
    public virtual void Close() => Frame.Close();

    /// <summary>
    /// Liga ou desliga o som (tecla Shift) fora da partida; na partida, o <c>ToggleMuteCommand</c> faz o mesmo.
    /// </summary>
    protected void ToggleSound()
    {
        Frame.Sound.ToggleMute();
        Frame.Invalidate();
    }
}
