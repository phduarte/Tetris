namespace Gadz.Tetris.Desktop.Device;

/// <summary>
/// Estilo do LCD, escolhido no menu e aplicado a todas as telas.
/// </summary>
internal enum Skin
{
    /// <summary>
    /// LCD esverdeado e monocromático, como um minigame de bolso.
    /// </summary>
    BrickGame,

    /// <summary>
    /// LCD escuro com as peças coloridas.
    /// </summary>
    Tetris
}

/// <summary>
/// As cores do LCD num estilo.
/// </summary>
/// <param name="Background">O fundo do LCD.</param>
/// <param name="Ink">A "tinta" dos segmentos e textos.</param>
/// <param name="GhostAlpha">Opacidade (0–255) das células e segmentos apagados, que ficam sempre visíveis como num display real.</param>
/// <param name="Colored">Se as peças são pintadas com as suas cores.</param>
internal sealed record LcdTheme(Color Background, Color Ink, int GhostAlpha, bool Colored)
{
    /// <summary>
    /// LCD do estilo BRICK GAME.
    /// </summary>
    private static readonly LcdTheme _brickGame = new(ColorTranslator.FromHtml("#b4bf9f"), ColorTranslator.FromHtml("#1d2618"), 23, false);

    /// <summary>
    /// LCD do estilo TETRIS.
    /// </summary>
    private static readonly LcdTheme _tetris = new(ColorTranslator.FromHtml("#11161a"), ColorTranslator.FromHtml("#dfe8e1"), 15, true);

    /// <summary>
    /// As cores do LCD no estilo informado.
    /// </summary>
    /// <param name="skin">O estilo escolhido.</param>
    /// <returns>O tema do LCD.</returns>
    public static LcdTheme For(Skin skin) => skin == Skin.Tetris ? _tetris : _brickGame;
}
