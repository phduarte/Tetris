using Gadz.Tetris.Application;

namespace Gadz.Tetris.Web.Input;

/// <summary>
/// As teclas do navegador. O navegador informa cada tecla pelo nome (<c>KeyboardEvent.key</c>), e o jogo, pelo enum
/// <see cref="Key"/> da Application (códigos do WinForms); esta tabela traduz um para o outro, sem <c>switch</c>,
/// para a partida executar os mesmos <c>Command</c>s do Desktop.
/// </summary>
public static class KeyMap
{
    /// <summary>
    /// Seta para a esquerda.
    /// </summary>
    public const string LEFT = "ArrowLeft";

    /// <summary>
    /// Seta para a direita.
    /// </summary>
    public const string RIGHT = "ArrowRight";

    /// <summary>
    /// Seta para cima.
    /// </summary>
    public const string UP = "ArrowUp";

    /// <summary>
    /// Seta para baixo.
    /// </summary>
    public const string DOWN = "ArrowDown";

    /// <summary>
    /// Barra de espaço (o navegador a informa como um espaço).
    /// </summary>
    public const string SPACE = " ";

    /// <summary>
    /// Enter.
    /// </summary>
    public const string ENTER = "Enter";

    /// <summary>
    /// Esc.
    /// </summary>
    public const string ESCAPE = "Escape";

    /// <summary>
    /// Shift (qualquer um dos dois).
    /// </summary>
    public const string SHIFT = "Shift";

    /// <summary>
    /// F1: ajuda, no menu.
    /// </summary>
    public const string F1 = "F1";

    /// <summary>
    /// F2: sobre, no menu. O Desktop usa F12, que no navegador abre as ferramentas do desenvolvedor.
    /// </summary>
    public const string F2 = "F2";

    /// <summary>
    /// As teclas do navegador que viram teclas do jogo.
    /// </summary>
    private static readonly Dictionary<string, Key> _gameKeys = new()
    {
        [LEFT] = Key.Left,
        [RIGHT] = Key.Right,
        [UP] = Key.Up,
        [DOWN] = Key.Down,
        [SPACE] = Key.Space,
        [ENTER] = Key.Enter,
        [ESCAPE] = Key.Escape,
        [SHIFT] = Key.ShiftKey,
    };

    /// <summary>
    /// Todas as teclas que o jogo trata. Só elas têm a ação padrão do navegador cancelada
    /// (as setas e o espaço rolariam a página; o Enter clicaria o último botão focado).
    /// </summary>
    public static IReadOnlyCollection<string> HandledKeys { get; } = [.. _gameKeys.Keys, F1, F2];

    /// <summary>
    /// Traduz a tecla do navegador para a tecla do jogo.
    /// </summary>
    /// <param name="key">A tecla, como em <c>KeyboardEvent.key</c>.</param>
    /// <param name="gameKey">A tecla do jogo correspondente.</param>
    /// <returns><c>true</c> se a tecla tem um equivalente no jogo.</returns>
    public static bool TryGetGameKey(string key, out Key gameKey) => _gameKeys.TryGetValue(key, out gameKey);
}
